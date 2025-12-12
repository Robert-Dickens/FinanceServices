using ByteLabs.FinanceServices.Services.Identity.Blazor.Menus;
using ByteLabs.Foundations.AspNetCore.Components.Web;
using ByteLabs.Foundations.AspNetCore.Components.Web.Routing;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.Identity.Blazor;

[DependsOn(
    typeof(IdentityServiceApplicationAbstractionsModule),
    typeof(AspNetCoreComponentsWebThemingModule),
    typeof(AbpAutoMapperModule)
    )]
public class IdentityServiceBlazorModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<IdentityServiceBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<IdentityServiceBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AspNetCoreNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new IdentityServiceMenuContributor());
        });

        Configure<AspNetCoreRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(IdentityServiceBlazorModule).Assembly);
        });
    }
}
