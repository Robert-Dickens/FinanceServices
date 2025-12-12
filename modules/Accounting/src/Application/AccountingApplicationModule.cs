using ByteLabs.Foundations.Application;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;
using ByteLabs.FinanceServices.Accounting.Domain;
using ByteLabs.Foundations.AutoMapper;

namespace ByteLabs.FinanceServices.Accounting;

[DependsOn(
    typeof(AccountingDomainModule),
    typeof(AccountingApplicationAbstractionsModule),
    typeof(PlatformApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class AccountingApplicationModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AccountingApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AccountingApplicationModule>(validate: true);
        });
    }
}
