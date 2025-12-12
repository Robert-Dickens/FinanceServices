using ByteLabs.FinanceServices.Services.FinanceServices.Domain.Context;
using ByteLabs.Foundations;
using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Domain;

[DependsOn(
    typeof(EntityFrameworkCoreModule),
    typeof(FinanceServicesServiceDomainModule)
)]
public class FinanceServicesServiceDomainContextModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        FinanceServicesServiceEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<FinanceServicesServiceDbContext>(options =>
        {
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
            
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
                    .GetRequiredService<FinanceServicesServicePendingMigrationsChecker>()
                    .CheckAndApplyDatabaseMigrationsAsync();
            }
    }
}
