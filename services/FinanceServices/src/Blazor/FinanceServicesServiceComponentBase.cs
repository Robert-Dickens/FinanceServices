using ByteLabs.FinanceServices.Services.FinanceServices.Localization;
using ByteLabs.Foundations.AspNetCore.Components;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Blazor
{
    public abstract class FinanceServicesServiceComponentBase : AspNetCoreComponentBase
    {
        protected FinanceServicesServiceComponentBase()
        {
            LocalizationResource = typeof(FinanceServicesServiceResource);
        }
    }
}
