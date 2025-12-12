using ByteLabs.FinanceServices.Recevables.Domain.Context;
using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Recevables.Domain;

[DependsOn(
    typeof(EntityFrameworkCoreModule),
    typeof(RecevablesDomainModule)
)]
public class RecevablesDomainContextModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        RecevablesEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<RecevablesDbContext>(options =>
        {
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
        });
    }
}
