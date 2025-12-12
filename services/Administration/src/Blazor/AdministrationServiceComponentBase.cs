using ByteLabs.FinanceServices.Services.Administration.Localization;
using ByteLabs.Foundations.AspNetCore.Components;

namespace ByteLabs.FinanceServices.Services.Administration.Blazor
{
    public abstract class AdministrationServiceComponentBase : AspNetCoreComponentBase
    {
        protected AdministrationServiceComponentBase()
        {
            LocalizationResource = typeof(AdministrationServiceResource);
        }
    }
}
