using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.Administration.Testing;

[DependsOn(
    typeof(AdministrationServiceApplicationModule),
    typeof(AdministrationServiceDomainTestModule)
    )]
public class AdministrationServiceApplicationTestModule : PlatformModule
{

}
