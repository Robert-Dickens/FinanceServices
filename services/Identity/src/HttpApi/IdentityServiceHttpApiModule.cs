using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.Account;
using ByteLabs.PlatformServices.Identity;
using ByteLabs.PlatformServices.Security.IdentityServer;

namespace ByteLabs.FinanceServices.Services.Identity;

[DependsOn(
    typeof(IdentityServiceApplicationAbstractionsModule),
    typeof(IdentityHttpApiModule),
    typeof(IdentityServerHttpApiModule),
    typeof(AccountAdminHttpApiModule)
    )]
public class IdentityServiceHttpApiModule : PlatformModule
{

}
