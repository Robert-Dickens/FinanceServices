using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;
using ByteLabs.FinanceServices.Accounting.Domain.Context;

namespace ByteLabs.FinanceServices.Accounting.Domain;

[DependsOn(
    typeof(EntityFrameworkCoreModule),
    typeof(AccountingDomainModule)
)]
public class AccountingDomainContextModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AccountingEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<AccountingDbContext>(options =>
        {
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);

        });
    }
}
