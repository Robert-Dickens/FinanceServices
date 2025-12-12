using ByteLabs.FinanceServices.Accounting.Blazor;
using ByteLabs.Foundations.AspNetCore.Components;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Accounting.Testing;

[DependsOn(
    typeof(AspNetCoreComponentsModule),
    typeof(AccountingDomainContextTestModule),
    typeof(AccountingBlazorModule)
)]
public class AccountingBlazorTestModule : PlatformModule
{

}
