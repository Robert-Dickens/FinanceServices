using ByteLabs.FinanceServices.Services.Identity.Localization;
using ByteLabs.Foundations.AspNetCore.Components;

namespace ByteLabs.FinanceServices.Services.Identity.Blazor
{
    public abstract class IdentityServiceComponentBase : AspNetCoreComponentBase
    {
        protected IdentityServiceComponentBase()
        {
            LocalizationResource = typeof(IdentityServiceResource);
        }
    }
}
