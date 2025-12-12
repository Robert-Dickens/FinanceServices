using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Payables.Testing;

[DependsOn(
    typeof(PayablesApplicationModule),
    typeof(PayablesDomainTestModule)
    )]
public class PayablesApplicationTestModule : PlatformModule
{

}
