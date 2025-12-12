using ByteLabs.Foundations.Auditing;
using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.SaaS.Domain;

namespace ByteLabs.FinanceServices.Services.Saas.Domain;

[DependsOn(
    typeof(AuditingModule),
    typeof(SaasServiceDomainAbstractionsModule),
    typeof(SaasDomainModule)
)]
public class SaasServiceDomainModule : PlatformModule
{
}
