using ByteLabs.Foundations.AspNetCore.Components.Server.Theming;
using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.SaaS.Blazor.Server;

namespace ByteLabs.FinanceServices.Services.Saas.Blazor;

[DependsOn(
    typeof(AspNetCoreComponentsServerThemingModule),
    typeof(SaasServiceBlazorModule),
    typeof(SaasBlazorServerModule)
    )]
public class SaasServiceBlazorServerModule : PlatformModule
{

}
