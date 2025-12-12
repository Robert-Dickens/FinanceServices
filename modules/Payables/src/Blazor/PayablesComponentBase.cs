using ByteLabs.FinanceServices.Payables.Localization;
using ByteLabs.Foundations.AspNetCore.Components;

namespace ByteLabs.FinanceServices.Payables.Blazor
{
    public abstract class PayablesComponentBase : AspNetCoreComponentBase
    {
        protected PayablesComponentBase()
        {
            LocalizationResource = typeof(PayablesResource);
        }
    }
}
