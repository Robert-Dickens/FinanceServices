using ByteLabs.Foundations.AspNetCore.Components.Server.Theming;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Platform.Foundations.Blazor.Server;

namespace ByteLabs.FinanceServices.Services.Administration.Blazor;

[DependsOn(
    typeof(AspNetCoreComponentsServerThemingModule),
    typeof(AdministrationServiceBlazorModule),
    typeof(PlatformServicesBlazorServerModule)
    )]
public class AdministrationServiceBlazorServerModule : PlatformModule
{

}
