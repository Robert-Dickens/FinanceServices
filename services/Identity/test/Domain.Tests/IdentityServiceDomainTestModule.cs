using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.Identity.Testing;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(IdentityServiceDomainContextTestModule)
    )]
public class IdentityServiceDomainTestModule : PlatformModule
{

}
