using ByteLabs.Foundations.AspNetCore.Components.Server.Theming;
using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.Identity.Blazor.Server;

namespace ByteLabs.FinanceServices.Services.Identity.Blazor;

[DependsOn(
    typeof(AspNetCoreComponentsServerThemingModule),
    typeof(IdentityServiceBlazorModule),
    typeof(IdentityBlazorServerModule)
    )]
public class IdentityServiceBlazorServerModule : PlatformModule
{

}
