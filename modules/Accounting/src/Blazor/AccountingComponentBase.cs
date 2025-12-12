using ByteLabs.Foundations.AspNetCore.Components;
using ByteLabs.FinanceServices.Accounting.Localization;

namespace ByteLabs.FinanceServices.Accounting.Blazor
{
    public abstract class AccountingComponentBase : AspNetCoreComponentBase
    {
        protected AccountingComponentBase()
        {
            LocalizationResource = typeof(AccountingResource);
        }
    }
}
