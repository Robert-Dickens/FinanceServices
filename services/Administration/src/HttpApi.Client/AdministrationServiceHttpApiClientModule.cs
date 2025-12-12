using ByteLabs.Foundations.Modularity;
using ByteLabs.Platform.Foundations;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.Administration;

[DependsOn(
    typeof(AdministrationServiceApplicationAbstractionsModule),
    typeof(PlatformServicesHttpApiClientModule)
)]
public class AdministrationServiceHttpApiClientModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(AdministrationServiceApplicationAbstractionsModule).Assembly,
            AdministrationServiceRemoteServiceConsts.RemoteServiceName
        );
    }
}
