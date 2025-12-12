using ByteLabs.FinanceServices.Services.Identity.Blazor;
using ByteLabs.Foundations.AspNetCore.Components;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.Identity.Testing;

[DependsOn(
    typeof(AspNetCoreComponentsModule),
    typeof(IdentityServiceDomainContextTestModule),
    typeof(IdentityServiceBlazorModule)
)]
public class IdentityServiceBlazorTestModule : PlatformModule
{

}
