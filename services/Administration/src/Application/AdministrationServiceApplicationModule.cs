using ByteLabs.FinanceServices.Services.Administration.Domain;
using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Platform.Foundations;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.Administration;

[DependsOn(
    typeof(AdministrationServiceApplicationAbstractionsModule),
    typeof(AdministrationServiceDomainModule),
    typeof(PlatformServicesApplicationModule)
)]
public class AdministrationServiceApplicationModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AdministrationServiceApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AdministrationServiceApplicationModule>(validate: true);
        });
    }
}
