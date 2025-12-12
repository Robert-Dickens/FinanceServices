using ByteLabs.FinanceServices.Payables.Domain.Context;
using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Payables.Domain;

[DependsOn(
    typeof(EntityFrameworkCoreModule),
    typeof(PayablesDomainModule)
)]
public class PayablesDomainContextModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PayablesEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<PayablesDbContext>(options =>
        {
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
            // options.AddRepository<Product, EfCoreProductRepository>();
        });
    }
}
