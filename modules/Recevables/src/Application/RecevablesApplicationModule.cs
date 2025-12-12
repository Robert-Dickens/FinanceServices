using ByteLabs.FinanceServices.Recevables.Domain;
using ByteLabs.Foundations.Application;
using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Recevables;

[DependsOn(
    typeof(RecevablesDomainModule),
    typeof(RecevablesApplicationAbstractionsModule),
    typeof(PlatformApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class RecevablesApplicationModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<RecevablesApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<RecevablesApplicationModule>(validate: true);
        });
    }
}
