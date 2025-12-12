using ByteLabs.Foundations.Modularity;
using ByteLabs.Platform.Foundations;

namespace ByteLabs.FinanceServices.Services.Administration;

[DependsOn(
    typeof(PlatformServicesApplicationAbstractionsModule),
    typeof(AdministrationServiceDomainAbstractionsModule)
)]
public class AdministrationServiceApplicationAbstractionsModule : PlatformModule
{
}
