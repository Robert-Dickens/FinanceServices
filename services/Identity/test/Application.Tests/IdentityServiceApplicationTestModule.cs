using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.Identity.Testing;

[DependsOn(
    typeof(IdentityServiceApplicationModule),
    typeof(IdentityServiceDomainTestModule)
    )]
public class IdentityServiceApplicationTestModule : PlatformModule
{

}
