using ByteLabs.Aps.Hosting.Gateways;
using ByteLabs.Foundations.Modularity;
using FinanceServices.Shared.Hosting.AspNetCore;

namespace FinanceServices.Shared.Hosting.Gateways;

[DependsOn(
    typeof(AspNetCoreHostingModule),
    typeof(PlatformHostingOcelotGatewaysModule)
)]
public class AspNetCoreOcelotGatewaysModule : PlatformModule
{
}
