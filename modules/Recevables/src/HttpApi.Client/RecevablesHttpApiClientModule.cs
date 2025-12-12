using ByteLabs.Foundations.Http.Client;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Recevables;

[DependsOn(
    typeof(RecevablesApplicationAbstractionsModule),
    typeof(HttpClientModule))]
public class RecevablesHttpApiClientModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(typeof(RecevablesApplicationAbstractionsModule).Assembly,
            RecevablesRemoteServiceConsts.RemoteServiceName);

    }
}
