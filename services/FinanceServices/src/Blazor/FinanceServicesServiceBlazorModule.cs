using ByteLabs.FinanceServices.Services.FinanceServices.Blazor.Menus;
using ByteLabs.Foundations.AspNetCore.Components.Web;
using ByteLabs.Foundations.AspNetCore.Components.Web.Routing;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Blazor;

[DependsOn(
    typeof(FinanceServicesServiceApplicationAbstractionsModule),
    typeof(AspNetCoreComponentsWebThemingModule),
    typeof(AbpAutoMapperModule)
    )]
public class FinanceServicesServiceBlazorModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<FinanceServicesServiceBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<FinanceServicesServiceBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AspNetCoreNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new FinanceServicesServiceMenuContributor());
        });

        Configure<AspNetCoreRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(FinanceServicesServiceBlazorModule).Assembly);
        });
    }
}
