using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Recevables.Testing;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(RecevablesDomainContextTestModule)
    )]
public class RecevablesDomainTestModule : PlatformModule
{

}
