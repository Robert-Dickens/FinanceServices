using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.Saas.Testing;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(SaasServiceDomainContextTestModule)
    )]
public class SaasServiceDomainTestModule : PlatformModule
{

}
