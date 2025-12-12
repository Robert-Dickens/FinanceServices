using ByteLabs.Foundations.Modularity;
using ByteLabs.Platform.Foundations;

namespace ByteLabs.FinanceServices.Services.Administration;

[DependsOn(
    typeof(AdministrationServiceApplicationAbstractionsModule),
    typeof(PlatformServicesHttpApiModule)
)]
public class AdministrationServiceHttpApiModule : PlatformModule
{
}
