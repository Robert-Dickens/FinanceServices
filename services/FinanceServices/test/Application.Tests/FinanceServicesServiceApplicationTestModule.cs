using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Testing;

[DependsOn(
    typeof(FinanceServicesServiceApplicationModule),
    typeof(FinanceServicesServiceDomainTestModule)
    )]
public class FinanceServicesServiceApplicationTestModule : PlatformModule
{

}
