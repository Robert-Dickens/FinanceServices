using ByteLabs.FinanceServices.Payables.Blazor.Menus;
using ByteLabs.Foundations.AspNetCore.Components.Web;
using ByteLabs.Foundations.AspNetCore.Components.Web.Routing;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Payables.Blazor;

[DependsOn(
    typeof(PayablesApplicationAbstractionsModule),
    typeof(AspNetCoreComponentsWebThemingModule),
    typeof(AbpAutoMapperModule)
    )]
public class PayablesBlazorModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<PayablesBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<PayablesBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AspNetCoreNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new PayablesMenuContributor());
        });

        Configure<AspNetCoreRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(PayablesBlazorModule).Assembly);
        });
    }
}
