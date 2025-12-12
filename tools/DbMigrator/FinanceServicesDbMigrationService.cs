using ByteLabs.FinanceServices.Services.Administration.Domain.Context;
using ByteLabs.FinanceServices.Services.FinanceServices.Domain.Context;
using ByteLabs.FinanceServices.Services.Identity.Domain;
using ByteLabs.FinanceServices.Services.Identity.Domain.Context;
using ByteLabs.FinanceServices.Services.Saas.Domain.Context;
using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.DependencyInjection;
using ByteLabs.Foundations.MultiTenancy;
using ByteLabs.Foundations.Uow;
using ByteLabs.PlatformServices.Identity.Domain.Data;
using ByteLabs.PlatformServices.SaaS.Domain.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DbMigrator;

public class FinanceServicesDbMigrationService : ITransientDependency
{
    private readonly ILogger<FinanceServicesDbMigrationService> _logger;
    private readonly IConfiguration _configuration;
    private readonly ITenantRepository _tenantRepository;
    private readonly IDataSeeder _dataSeeder;
    private readonly ICurrentTenant _currentTenant;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public FinanceServicesDbMigrationService(
        ILogger<FinanceServicesDbMigrationService> logger,
        IConfiguration configuration,
        ITenantRepository tenantRepository,
        IDataSeeder dataSeeder,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _logger = logger;
        _configuration = configuration;
        _tenantRepository = tenantRepository;
        _dataSeeder = dataSeeder;
        _currentTenant = currentTenant;
        _unitOfWorkManager = unitOfWorkManager;
    }

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await MigrateHostAsync(cancellationToken);
        await MigrateTenantsAsync(cancellationToken);
        _logger.LogInformation("Migration completed!");
    }

    private async Task MigrateHostAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Migrating Host side...");
        await MigrateAllDatabasesAsync(null, cancellationToken);
        await SeedDataAsync();
    }

    private async Task MigrateTenantsAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Migrating tenants...");

        var tenants =
            await _tenantRepository.GetListAsync(includeDetails: true, cancellationToken: cancellationToken);
        var migratedDatabaseSchemas = new HashSet<string>();
        foreach (var tenant in tenants)
        {
            using (_currentTenant.Change(tenant.Id))
            {
                // Database schema migration
                var connectionString = tenant.FindDefaultConnectionString();
                if (!connectionString.IsNullOrWhiteSpace() && //tenant has a separate database
                    !migratedDatabaseSchemas.Contains(connectionString)) //the database was not migrated yet
                {
                    _logger.LogInformation("Migrating tenant database: {TenantName} ({TenantId})", tenant.Name, tenant.Id);
                    await MigrateAllDatabasesAsync(tenant.Id, cancellationToken);
                    migratedDatabaseSchemas.AddIfNotContains(connectionString);
                }

                //Seed data
                _logger.LogInformation("Seeding tenant data: {TenantName} ({TenantId})", tenant.Name, tenant.Id);
                await SeedDataAsync();
            }
        }
    }

    private async Task MigrateAllDatabasesAsync(
        Guid? tenantId,
        CancellationToken cancellationToken)
    {
        using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: false))
        {
            if (tenantId == null)
            {
                /* SaaS schema should only be available in the host side */
                await MigrateDatabaseAsync<SaasServiceDbContext>(cancellationToken);
            }

            await MigrateDatabaseAsync<AdministrationServiceDbContext>(cancellationToken);
            await MigrateDatabaseAsync<IdentityServiceDbContext>(cancellationToken);
            await MigrateDatabaseAsync<FinanceServicesServiceDbContext>(cancellationToken);

            await uow.CompleteAsync(cancellationToken);
        }

        _logger.LogInformation("All databases have been successfully migrated ({(TenantOrHost)}).", tenantId.HasValue ? $"tenantId: {tenantId}" : "HOST");
    }

    private async Task MigrateDatabaseAsync<TDbContext>(
        CancellationToken cancellationToken)
        where TDbContext : DbContext, IEfCoreDbContext
    {
        _logger.LogInformation("Migrating {DbContextName} database...", typeof(TDbContext).Name.RemovePostFix("DbContext"));

        var dbContext = await _unitOfWorkManager.Current.ServiceProvider
            .GetRequiredService<IDbContextProvider<TDbContext>>()
            .GetDbContextAsync();

        await dbContext
            .Database
            .MigrateAsync(cancellationToken);
    }

    private async Task SeedDataAsync()
    {
        var defaultUsername = _configuration.GetValue("Defaults:AdminUserName", IdentityServiceDbProperties.DefaultAdminEmailAddress);
        var defaultPassword = _configuration.GetValue("Defaults:AdminPassword", IdentityServiceDbProperties.DefaultAdminPassword);
        await _dataSeeder.SeedAsync(
            new DataSeedContext(_currentTenant.Id)
                .WithProperty(IdentityDataSeedContributor.AdminEmailPropertyName, defaultUsername)
                .WithProperty(IdentityDataSeedContributor.AdminPasswordPropertyName, defaultPassword)
        );
    }
}
