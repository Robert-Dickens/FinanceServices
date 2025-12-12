using ByteLabs.Foundations.AspNetCore.Components;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Testing;

[DependsOn(
    typeof(AspNetCoreComponentsModule),
    typeof(FinanceServicesServiceDomainContextTestModule),
    typeof(FinanceServicesServiceBlazorModule)
)]
public class FinanceServicesServiceBlazorTestModule : PlatformModule
{

}
