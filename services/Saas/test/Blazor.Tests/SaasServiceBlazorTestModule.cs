using ByteLabs.FinanceServices.Services.Saas.Blazor;
using ByteLabs.Foundations.AspNetCore.Components;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.Saas.Testing;

[DependsOn(
    typeof(AspNetCoreComponentsModule),
    typeof(SaasServiceDomainContextTestModule),
    typeof(SaasServiceBlazorModule)
)]
public class SaasServiceBlazorTestModule : PlatformModule
{

}
