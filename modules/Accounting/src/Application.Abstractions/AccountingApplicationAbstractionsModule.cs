using ByteLabs.Foundations.Application;
using ByteLabs.Foundations.Authorization;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Accounting;

[DependsOn(
    typeof(AccountingDomainAbstractionsModule),
    typeof(PlatformApplicationAbstractionsModule),
    typeof(PlatformAuthorizationModule)
    )]
public class AccountingApplicationAbstractionsModule : PlatformModule
{

}
