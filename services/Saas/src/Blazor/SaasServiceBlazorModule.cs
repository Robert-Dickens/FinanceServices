using ByteLabs.FinanceServices.Services.Saas.Blazor.Menus;
using ByteLabs.Foundations.AspNetCore.Components.Web;
using ByteLabs.Foundations.AspNetCore.Components.Web.Routing;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.Saas.Blazor;

[DependsOn(
    typeof(SaasServiceApplicationAbstractionsModule),
    typeof(AspNetCoreComponentsWebThemingModule),
    typeof(AbpAutoMapperModule)
    )]
public class SaasServiceBlazorModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<SaasServiceBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<SaasServiceBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AspNetCoreNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new SaasServiceMenuContributor());
        });

        Configure<AspNetCoreRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(SaasServiceBlazorModule).Assembly);
        });
    }
}
