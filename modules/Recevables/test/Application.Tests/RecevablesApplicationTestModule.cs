using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Recevables.Testing;

[DependsOn(
    typeof(RecevablesApplicationModule),
    typeof(RecevablesDomainTestModule)
    )]
public class RecevablesApplicationTestModule : PlatformModule
{

}
