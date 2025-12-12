using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.Account;
using ByteLabs.PlatformServices.AuditLogging;
using ByteLabs.PlatformServices.Identity;
using ByteLabs.PlatformServices.Security.IdentityServer;

namespace ByteLabs.FinanceServices.Services.Identity;

[DependsOn(
    typeof(IdentityApplicationAbstractionsModule),
    typeof(IdentityServerApplicationAbstractionsModule),
    typeof(AccountAdminApplicationAbstractionsModule),
    typeof(IdentityServiceDomainAbstractionsModule),
    typeof(AuditLoggingApplicationAbstractionsModule)
)]
public class IdentityServiceApplicationAbstractionsModule : PlatformModule
{
}
