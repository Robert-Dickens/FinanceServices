using ByteLabs.FinanceServices.Accounting;
using ByteLabs.FinanceServices.Payables;
using ByteLabs.FinanceServices.Recevables;
using ByteLabs.Foundations.Http.Client;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.FinanceServices;

[DependsOn(
    typeof(FinanceServicesServiceApplicationAbstractionsModule),
    typeof(AccountingHttpApiClientModule),
    typeof(PayablesHttpApiClientModule),
    typeof(RecevablesHttpApiClientModule),
    typeof(HttpClientModule))]
public class FinanceServicesServiceHttpApiClientModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(typeof(FinanceServicesServiceApplicationAbstractionsModule).Assembly,
            FinanceServicesServiceRemoteServiceConsts.RemoteServiceName);

    }
}
