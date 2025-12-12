using ByteLabs.Foundations.Http.Client;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.FinanceServices;

[DependsOn(
    typeof(FinanceServicesServiceApplicationAbstractionsModule),
    typeof(HttpClientModule))]
public class FinanceServicesServiceHttpApiClientModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(typeof(FinanceServicesServiceApplicationAbstractionsModule).Assembly,
            FinanceServicesServiceRemoteServiceConsts.RemoteServiceName);

    }
}
