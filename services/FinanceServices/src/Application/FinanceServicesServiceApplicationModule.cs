using ByteLabs.FinanceServices.Accounting;
using ByteLabs.FinanceServices.Payables;
using ByteLabs.FinanceServices.Recevables;
using ByteLabs.FinanceServices.Services.FinanceServices.Domain;
using ByteLabs.Foundations.Application;
using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.FinanceServices;

[DependsOn(
    typeof(FinanceServicesServiceDomainModule),
    typeof(FinanceServicesServiceApplicationAbstractionsModule),
    typeof(AccountingApplicationModule),
    typeof(PayablesApplicationModule),
    typeof(RecevablesApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class FinanceServicesServiceApplicationModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<FinanceServicesServiceApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<FinanceServicesServiceApplicationModule>(validate: true);
        });
    }
}
