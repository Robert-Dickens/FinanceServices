using ByteLabs.Foundations.AspNetCore.Components.WebAssembly.Theming;
using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.SaaS.Blazor.WebAssembly;

namespace ByteLabs.FinanceServices.Services.Saas.Blazor;

[DependsOn(
    typeof(SaasServiceBlazorModule),
    typeof(SaasServiceHttpApiClientModule),
    typeof(AspNetCoreComponentsWebAssemblyThemingModule),
    typeof(SaasBlazorWebAssemblyModule)
    )]
public class SaasServiceBlazorWebAssemblyModule : PlatformModule
{

}
