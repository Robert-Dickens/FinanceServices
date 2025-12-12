using ByteLabs.FinanceServices.Payables.Domain;
using ByteLabs.Foundations.Application;
using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Payables;

[DependsOn(
    typeof(PayablesDomainModule),
    typeof(PayablesApplicationAbstractionsModule),
    typeof(PlatformApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class PayablesApplicationModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<PayablesApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<PayablesApplicationModule>(validate: true);
        });
    }
}
