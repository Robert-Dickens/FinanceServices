using ByteLabs.Foundations.Auditing;
using ByteLabs.Foundations.Domain;
using ByteLabs.Foundations.Features;
using ByteLabs.Foundations.GlobalFeatures;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Domain;

[DependsOn(
    typeof(PlatformDomainModule),
    typeof(AuditingModule),
    typeof(GlobalFeaturesModule),
    typeof(FeaturesModule),
    typeof(FinanceServicesServiceDomainAbstractionsModule)
)]
public class FinanceServicesServiceDomainModule : PlatformModule
{

}
