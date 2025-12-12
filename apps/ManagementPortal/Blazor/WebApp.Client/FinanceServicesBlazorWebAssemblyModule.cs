using ByteLabs.FinanceServices.Services.Administration.Blazor;
using ByteLabs.FinanceServices.Services.FinanceServices.Blazor;
using ByteLabs.Foundations.AspNetCore.Components.Web;
using ByteLabs.Foundations.AspNetCore.Components.WebAssembly;
using ByteLabs.Foundations.AspNetCore.Components.WebAssembly.Theming;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.AspNetCore.WebAssembly;
using ByteLabs.Foundations.Authorization.TokenManagement.WebAssembly;
using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.Modularity;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FinanceServices.ManagementPortal.Blazor.WebAssembly.Menus;
using FinanceServices.Shared;
using ByteLabs.Foundations.Autofac.WebAssembly;
using ByteLabs.Foundations.AspNetCore.Components.Web.Routing;

namespace FinanceServices.ManagementPortal.Blazor.WebAssembly;

[DependsOn(
    typeof(AutofacWebAssemblyModule),
    typeof(AspNetCoreComponentsWebAssemblyModule),
    typeof(AspNetCoreComponentsWebAssemblyThemingModule),
    typeof(AspNetCoreFluentDesignWebAssemblyThemeModule),
    typeof(AdministrationServiceWebAssemblyModule),
    typeof(SaasServiceBlazorWebAssemblyModule),
    typeof(IdentityServiceBlazorWebAssemblyModule),
    typeof(FinanceServicesServiceWebAssemblyModule),
    typeof(SharedLocalizationModule)
)]
public class FinanceServicesBlazorWebAssemblyModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AspNetCoreComponentsWebOptions>(options =>
        {
            options.IsBlazorWebApp = true;
        });

        PreConfigure<WebAssemblyTokenManagementProviderOptions>(options =>
        {
            options.TokenProviderType = ProviderType.PersistentComponentState;
        });

    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var environment = context.Services.GetSingletonInstance<IWebAssemblyHostEnvironment>();
        var builder = context.Services.GetSingletonInstance<WebAssemblyHostBuilder>();

        ConfigureHttpClient(context, environment);
        ConfigureRouter(context);
        ConfigureMenu(context);
        ConfigureAutoMapper(context);

        context.Services.AddAuthorizationCore();
        context.Services.AddCascadingAuthenticationState();
        context.Services.AddBlazorWebAppTieredServices();

    }

      private void ConfigureRouter(ServiceConfigurationContext context)
    {
        Configure<AspNetCoreRouterOptions>(options =>
        {
            options.AppAssembly = typeof(FinanceServicesBlazorWebAssemblyModule).Assembly;
        });
    }

    private void ConfigureMenu(ServiceConfigurationContext context)
    {
        Configure<AspNetCoreNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new FinanceServicesMenuContributor(context.Services.GetConfiguration()));
        });
    }

    private static void ConfigureHttpClient(ServiceConfigurationContext context, IWebAssemblyHostEnvironment environment)
    {
        context.Services.AddTransient(sp => new HttpClient
        {
            BaseAddress = new Uri(environment.BaseAddress)
        });
    }

    private void ConfigureAutoMapper(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<FinanceServicesBlazorWebAssemblyModule>();
        });
    }
}
