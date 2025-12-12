using ByteLabs.FinanceServices.Services.Administration.Domain.Context;
using ByteLabs.Foundations;
using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Platform.Foundations.Domain.Context;
using ByteLabs.PlatformServices.Features.Domain;
using ByteLabs.PlatformServices.Permissions.Domain;
using ByteLabs.PlatformServices.Settings.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.Administration.Domain;

[DependsOn(
    typeof(AdministrationServiceDomainModule),
    typeof(EntityFrameworkCoreModule),
    typeof(PlatformServicesDomainContextModule)
)]
public class AdministrationServiceDomainContextModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AdministrationServiceEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<AdministrationServiceDbContext>(options =>
        {
            options.ReplaceDbContext<IPlatformServicesDbContext>();
            /* includeAllEntities: true allows to use IRepository<TEntity, TKey> also for non aggregate root entities */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        Configure<FeatureManagementOptions>(options => { options.SaveStaticFeaturesToDatabase = true; });
        Configure<SettingManagementOptions>(options => { options.SaveStaticSettingsToDatabase = true; });
        Configure<PermissionManagementOptions>(options => { options.SaveStaticPermissionsToDatabase = true; });

        Configure<AppDbConnectionOptions>(options =>
        {
            options.Databases.Configure(AdministrationServiceDbProperties.ConnectionStringName, database =>
            {
                database.MappedConnections.Add(PlatformServicesDbProperties.ConnectionStringName);
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
                    .GetRequiredService<AdministrationServicePendingMigrationsChecker>()
                    .CheckAndApplyDatabaseMigrationsAsync();
            }
    }
}
