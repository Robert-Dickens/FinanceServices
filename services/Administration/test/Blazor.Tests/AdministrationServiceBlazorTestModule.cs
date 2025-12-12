using ByteLabs.FinanceServices.Services.Administration.Blazor;
using ByteLabs.Foundations.AspNetCore.Components;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.Administration.Testing;

[DependsOn(
    typeof(AspNetCoreComponentsModule),
    typeof(AdministrationServiceDomainContextTestModule),
    typeof(AdministrationServiceBlazorModule)
)]
public class AdministrationServiceBlazorTestModule : PlatformModule
{

}
