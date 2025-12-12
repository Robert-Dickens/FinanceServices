using ByteLabs.Foundations.AspNetCore.Components.WebAssembly.Theming;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Platform.Foundations.Blazor.WebAssembly;

namespace ByteLabs.FinanceServices.Services.Administration.Blazor;

[DependsOn(
    typeof(AdministrationServiceBlazorModule),
    typeof(AdministrationServiceHttpApiClientModule),
    typeof(AspNetCoreComponentsWebAssemblyThemingModule),
    typeof(PlatformServicesBlazorWebAssemblyModule)
    )]
public class AdministrationServiceWebAssemblyModule : PlatformModule
{

}
