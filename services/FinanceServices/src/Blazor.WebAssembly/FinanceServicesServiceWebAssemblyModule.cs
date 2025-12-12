using ByteLabs.Foundations.AspNetCore.Components.WebAssembly.Theming;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Blazor;

[DependsOn(
    typeof(FinanceServicesServiceBlazorModule),
    typeof(FinanceServicesServiceHttpApiClientModule),
    typeof(AspNetCoreComponentsWebAssemblyThemingModule)
    )]
public class FinanceServicesServiceWebAssemblyModule : PlatformModule
{

}
