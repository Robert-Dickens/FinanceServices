using ByteLabs.FinanceServices.Recevables.Blazor.Menus;
using ByteLabs.Foundations.AspNetCore.Components.Web;
using ByteLabs.Foundations.AspNetCore.Components.Web.Routing;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Recevables.Blazor;

[DependsOn(
    typeof(RecevablesApplicationAbstractionsModule),
    typeof(AspNetCoreComponentsWebThemingModule),
    typeof(AbpAutoMapperModule)
    )]
public class RecevablesBlazorModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<RecevablesBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<RecevablesBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AspNetCoreNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new RecevablesMenuContributor());
        });

        Configure<AspNetCoreRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(RecevablesBlazorModule).Assembly);
        });
    }
}
