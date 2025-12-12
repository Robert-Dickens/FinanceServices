using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.AuditLogging;
using ByteLabs.PlatformServices.SaaS;

namespace ByteLabs.FinanceServices.Services.Saas;

[DependsOn(
    typeof(SaasApplicationAbstractionsModule),
    typeof(SaasServiceDomainAbstractionsModule),
    typeof(AuditLoggingApplicationAbstractionsModule)
)]
public class SaasServiceApplicationAbstractionsModule : PlatformModule
{
}
