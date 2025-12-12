using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.SaaS.Client;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.Saas;

[DependsOn(
    typeof(SaasServiceApplicationAbstractionsModule),
    typeof(SaasHttpApiClientModule)
)]
public class SaasServiceHttpApiClientModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(SaasServiceApplicationAbstractionsModule).Assembly,
            SaasServiceRemoteServiceConsts.RemoteServiceName
        );
    }
}
