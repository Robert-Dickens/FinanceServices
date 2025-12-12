using ByteLabs.Foundations.AspNetCore.Components.WebAssembly.Theming;
using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.Identity.Blazor.WebAssembly;

namespace ByteLabs.FinanceServices.Services.Identity.Blazor;

[DependsOn(
    typeof(IdentityServiceBlazorModule),
    typeof(IdentityServiceHttpApiClientModule),
    typeof(AspNetCoreComponentsWebAssemblyThemingModule),
    typeof(IdentityBlazorWebAssemblyModule)
    )]
public class IdentityServiceBlazorWebAssemblyModule : PlatformModule
{

}
