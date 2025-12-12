using ByteLabs.FinanceServices.Services.Saas.Localization;
using ByteLabs.Foundations.AspNetCore.Components;

namespace ByteLabs.FinanceServices.Services.Saas.Blazor
{
    public abstract class SaasServiceComponentBase : AspNetCoreComponentBase
    {
        protected SaasServiceComponentBase()
        {
            LocalizationResource = typeof(SaasServiceResource);
        }
    }
}
