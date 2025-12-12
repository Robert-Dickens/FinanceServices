using ByteLabs.FinanceServices.Services.Saas.Domain.Context;
using ByteLabs.Foundations;
using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.SaaS.Domain;
using ByteLabs.PlatformServices.SaaS.Domain.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.Saas.Domain;

[DependsOn(
    typeof(SaasServiceDomainModule),
    typeof(SaasDomainContextModule),
    typeof(EntityFrameworkCoreModule)
)]
public class SaasServiceDomainContextModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        SaasServiceEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<SaasServiceDbContext>(options =>
        {
            options.ReplaceDbContext<ISaasDbContext>();

            /* includeAllEntities: true allows to use IRepository<TEntity, TKey> also for non aggregate root entities */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        Configure<AppDbConnectionOptions>(options =>
        {
            options.Databases.Configure(SaasServiceDbProperties.ConnectionStringName, database =>
            {
                database.MappedConnections.Add(SaasDbProperties.ConnectionStringName);
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
                    .GetRequiredService<SaasServicePendingMigrationsChecker>()
                    .CheckAndApplyDatabaseMigrationsAsync();
            }
    }
}
