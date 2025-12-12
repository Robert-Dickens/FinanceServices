using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Accounting.Testing;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(AccountingDomainContextTestModule)
    )]
public class AccountingDomainTestModule : PlatformModule
{

}
