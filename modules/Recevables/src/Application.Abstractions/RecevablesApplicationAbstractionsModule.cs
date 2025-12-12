using ByteLabs.Foundations.Application;
using ByteLabs.Foundations.Authorization;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Recevables;

[DependsOn(
    typeof(RecevablesDomainAbstractionsModule),
    typeof(PlatformApplicationAbstractionsModule),
    typeof(PlatformAuthorizationModule)
    )]
public class RecevablesApplicationAbstractionsModule : PlatformModule
{

}
