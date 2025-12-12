using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Accounting.Testing;

[DependsOn(
    typeof(AccountingApplicationModule),
    typeof(AccountingDomainTestModule)
    )]
public class AccountingApplicationTestModule : PlatformModule
{

}
