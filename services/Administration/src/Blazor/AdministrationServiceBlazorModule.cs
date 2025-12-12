using ByteLabs.FinanceServices.Services.Administration.Blazor.Menus;
using ByteLabs.Foundations.AspNetCore.Components.Web.Routing;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Platform.Foundations.Blazor;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.Administration.Blazor;

[DependsOn(
    typeof(AdministrationServiceApplicationAbstractionsModule),
    typeof(PlatformServicesBlazorModule)
    )]
public class AdministrationServiceBlazorModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AdministrationServiceBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AdministrationServiceBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AspNetCoreNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AdministrationServiceMenuContributor());
        });

        Configure<AspNetCoreRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(AdministrationServiceBlazorModule).Assembly);
        });
    }
}
