using ByteLabs.Aps.Hosting.Gateways;
using ByteLabs.FinanceServices.Hosting.AspNetCore;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Hosting.Gateways;

[DependsOn(
    typeof(AspNetCoreHostingModule),
    typeof(PlatformHostingYarpGatewaysModule)
)]
public class AspNetCoreYarpGatewaysModule : PlatformModule
{
}