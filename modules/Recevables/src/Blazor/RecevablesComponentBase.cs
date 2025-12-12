using ByteLabs.FinanceServices.Recevables.Localization;
using ByteLabs.Foundations.AspNetCore.Components;

namespace ByteLabs.FinanceServices.Recevables.Blazor
{
    public abstract class RecevablesComponentBase : AspNetCoreComponentBase
    {
        protected RecevablesComponentBase()
        {
            LocalizationResource = typeof(RecevablesResource);
        }
    }
}
