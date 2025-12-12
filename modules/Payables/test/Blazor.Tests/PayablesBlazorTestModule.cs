using ByteLabs.FinanceServices.Payables.Blazor;
using ByteLabs.Foundations.AspNetCore.Components;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Payables.Testing;

[DependsOn(
    typeof(AspNetCoreComponentsModule),
    typeof(PayablesDomainContextTestModule),
    typeof(PayablesBlazorModule)
)]
public class PayablesBlazorTestModule : PlatformModule
{

}
