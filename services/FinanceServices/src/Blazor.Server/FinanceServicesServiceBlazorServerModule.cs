using ByteLabs.Foundations.AspNetCore.Components.Server.Theming;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Blazor;

[DependsOn(
    typeof(AspNetCoreComponentsServerThemingModule),
    typeof(FinanceServicesServiceBlazorModule)
    )]
public class FinanceServicesServiceBlazorServerModule : PlatformModule
{

}
