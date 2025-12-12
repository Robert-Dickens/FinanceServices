using ByteLabs.FinanceServices.Services.Saas.Domain;
using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.SaaS;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.Saas;

[DependsOn(
    typeof(SaasServiceApplicationAbstractionsModule),
    typeof(SaasServiceDomainModule),
    typeof(SaasApplicationModule)
)]
public class SaasServiceApplicationModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<SaasServiceApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<SaasServiceApplicationModule>(validate: true);
        });
    }
}
