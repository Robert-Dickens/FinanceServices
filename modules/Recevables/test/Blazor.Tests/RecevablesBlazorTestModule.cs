using ByteLabs.FinanceServices.Recevables.Blazor;
using ByteLabs.Foundations.AspNetCore.Components;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Recevables.Testing;

[DependsOn(
    typeof(AspNetCoreComponentsModule),
    typeof(RecevablesDomainContextTestModule),
    typeof(RecevablesBlazorModule)
)]
public class RecevablesBlazorTestModule : PlatformModule
{

}
