using ByteLabs.Foundations.Http.Client;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Accounting;

[DependsOn(
    typeof(AccountingApplicationAbstractionsModule),
    typeof(HttpClientModule))]
public class AccountingHttpApiClientModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(typeof(AccountingApplicationAbstractionsModule).Assembly,
            AccountingRemoteServiceConsts.RemoteServiceName);

    }
}
