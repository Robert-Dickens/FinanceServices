using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Payables.Testing;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(PayablesDomainContextTestModule)
    )]
public class PayablesDomainTestModule : PlatformModule
{

}
