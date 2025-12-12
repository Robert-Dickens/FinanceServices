using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.Saas.Testing;

[DependsOn(
    typeof(SaasServiceApplicationModule),
    typeof(SaasServiceDomainTestModule)
    )]
public class SaasServiceApplicationTestModule : PlatformModule
{

}
