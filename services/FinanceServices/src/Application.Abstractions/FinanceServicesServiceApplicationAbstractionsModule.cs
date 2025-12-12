using ByteLabs.Foundations.Application;
using ByteLabs.Foundations.Authorization;
using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.AuditLogging;

namespace ByteLabs.FinanceServices.Services.FinanceServices;

[DependsOn(
    typeof(FinanceServicesServiceDomainAbstractionsModule),
    typeof(PlatformApplicationAbstractionsModule),
    typeof(AuditLoggingApplicationAbstractionsModule),
    typeof(PlatformAuthorizationModule)
    )]
public class FinanceServicesServiceApplicationAbstractionsModule : PlatformModule
{

}
