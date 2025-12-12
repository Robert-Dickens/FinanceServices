using ByteLabs.Foundations.AspNetCore.Mvc.UI;
using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.SaaS;

namespace ByteLabs.FinanceServices.Services.Saas;

[DependsOn(
    typeof(SaasServiceApplicationAbstractionsModule),
    typeof(SaasHttpApiModule),
    typeof(AspNetCoreMvcMultiTenancyModule)
)]
public class SaasServiceHttpApiModule : PlatformModule
{
}
