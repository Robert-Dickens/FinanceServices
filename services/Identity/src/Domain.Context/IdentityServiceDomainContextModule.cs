using ByteLabs.FinanceServices.Services.Identity.Domain.Context;
using ByteLabs.Foundations;
using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.Identity.Domain;
using ByteLabs.PlatformServices.Security.IdentityServer.Domain;
using ByteLabs.PlatformServices.Security.IdentityServer.Domain.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.Identity.Domain;

[DependsOn(
    typeof(IdentityServiceDomainModule),
    typeof(IdentityDomainContextModule),
    typeof(IdentityServerDomainContextModule),
    typeof(EntityFrameworkCoreModule)
)]
public class IdentityServiceDomainContextModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        IdentityServiceEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<IdentityServiceDbContext>(options =>
        {
            options.ReplaceDbContext<IIdentityDbContext>();
            options.ReplaceDbContext<IIdentityServerDbContext>();

            /* includeAllEntities: true allows to use IRepository<TEntity, TKey> also for non aggregate root entities */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        Configure<AppDbConnectionOptions>(options =>
        {
            options.Databases.Configure(IdentityServiceDbProperties.ConnectionStringName, database =>
            {
                database.MappedConnections.Add(IdentityDbProperties.ConnectionStringName);
                database.MappedConnections.Add(IdentityServerDbProperties.ConnectionStringName);
                database.IsUsedByTenants = false;
            });

        });
    }

    public async override Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var canRunPendingMigrations = context.ServiceProvider.GetRequiredService<IConfiguration>().GetValue<bool>("HostingOptions:EnablePendingMigrations", true);
        var isDbMigrationEnvironment = context.ServiceProvider.IsDataMigrationEnvironment();
        if (canRunPendingMigrations && !isDbMigrationEnvironment)
            using (var scope = context.ServiceProvider.CreateScope())
            {
                await scope.ServiceProvider
                    .GetRequiredService<IdentityServicePendingMigrationsChecker>()
                    .CheckAndApplyDatabaseMigrationsAsync();
            }
    }
}
