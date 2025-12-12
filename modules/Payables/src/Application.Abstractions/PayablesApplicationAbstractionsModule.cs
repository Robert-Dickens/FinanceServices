using ByteLabs.Foundations.Application;
using ByteLabs.Foundations.Authorization;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Payables;

[DependsOn(
    typeof(PayablesDomainAbstractionsModule),
    typeof(PlatformApplicationAbstractionsModule),
    typeof(PlatformAuthorizationModule)
    )]
public class PayablesApplicationAbstractionsModule : PlatformModule
{

}
