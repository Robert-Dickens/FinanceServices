using ByteLabs.Foundations.Http.Client;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Payables;

[DependsOn(
    typeof(PayablesApplicationAbstractionsModule),
    typeof(HttpClientModule))]
public class PayablesHttpApiClientModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(typeof(PayablesApplicationAbstractionsModule).Assembly,
            PayablesRemoteServiceConsts.RemoteServiceName);

    }
}
