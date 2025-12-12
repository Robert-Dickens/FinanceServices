using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.Account;
using ByteLabs.PlatformServices.Identity.Client;
using ByteLabs.PlatformServices.Security.IdentityServer;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.Identity;

[DependsOn(
    typeof(IdentityServiceApplicationAbstractionsModule),
    typeof(IdentityServerHttpApiClientModule),
    typeof(IdentityHttpApiClientModule),
    typeof(AccountAdminHttpApiClientModule)
    )]
public class IdentityServiceHttpApiClientModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(IdentityServiceApplicationAbstractionsModule).Assembly,
            IdentityServiceRemoteServiceConsts.RemoteServiceName
        );
    }
}
