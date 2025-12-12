using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.DependencyInjection;
using ByteLabs.Foundations.MultiTenancy;
using ByteLabs.Foundations.Uow;
using ByteLabs.PlatformServices.Identity.Domain.Data;
using ByteLabs.PlatformServices.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ByteLabs.FinanceServices.Services.Identity.Domain;

public class IdentityServiceDataSeeder : ITransientDependency
{
    private readonly ILogger<IdentityServiceDataSeeder> _logger;
    private readonly IIdentityDataSeeder _identityDataSeeder;
    protected IEnumerable<IOpenApiDataSeedContributor> OpenDataSeedContributors { get; }
    protected OpenApiDataSeedOptions OpenApiOptions { get; }

    private readonly ICurrentTenant _currentTenant;
    private readonly IConfiguration _configuration;

    public IdentityServiceDataSeeder(
        IIdentityDataSeeder identityDataSeeder,
        ICurrentTenant currentTenant,
        IConfiguration configuration,
        ILogger<IdentityServiceDataSeeder> logger,
        IEnumerable<IOpenApiDataSeedContributor> openDataSeedContributors,
        IOptions<OpenApiDataSeedOptions> openApiOptions)
    {
        _identityDataSeeder = identityDataSeeder;
        _currentTenant = currentTenant;
        _configuration = configuration;
        _logger = logger;
        OpenDataSeedContributors = openDataSeedContributors;
        OpenApiOptions = openApiOptions.Value;
    }

    public async Task SeedAsync()
    {
        try
        {
            var defaultUsername = _configuration.GetValue("Defaults:AdminUserName", IdentityServiceDbProperties.DefaultAdminEmailAddress);
            var defaultPassword = _configuration.GetValue("Defaults:AdminPassword", IdentityServiceDbProperties.DefaultAdminPassword);

            await SeedAsync(null, defaultUsername, defaultPassword);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    public virtual async Task SeedIdentityServerAsync(Guid? tenantId, string adminEmail, string adminPassword)
    {
        foreach (var apiDataSeedContributor in OpenDataSeedContributors)
        {
            await apiDataSeedContributor.SeedAsync(new DataSeedContext(tenantId), OpenApiOptions);
        }
    }

    [UnitOfWork]
    public async Task SeedAsync(Guid? tenantId, string adminEmail, string adminPassword)
    {
        try
        {
            using (_currentTenant.Change(tenantId))
            {
                _logger.LogInformation("Seeding IdentityServer data...");
                await SeedIdentityServerAsync(tenantId, adminEmail, adminPassword);

                _logger.LogInformation("Seeding Identity data...");
                await _identityDataSeeder.SeedAsync(
                    adminEmail,
                    adminPassword,
                    tenantId
                );
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}
