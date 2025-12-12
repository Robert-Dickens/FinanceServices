using ByteLabs.FinanceServices.Accounting.Domain;
using ByteLabs.FinanceServices.Payables.Domain;
using ByteLabs.FinanceServices.Recevables.Domain;
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
    typeof(FinanceServicesServiceDomainAbstractionsModule),
    typeof(AccountingDomainModule),
    typeof(PayablesDomainModule),
    typeof(RecevablesDomainModule)
)]
public class FinanceServicesServiceDomainModule : PlatformModule
{

}
