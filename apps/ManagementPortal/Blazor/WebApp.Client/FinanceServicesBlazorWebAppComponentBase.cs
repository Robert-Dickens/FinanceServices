using ByteLabs.FinanceServices.Localization.Localization;
using ByteLabs.Foundations.AspNetCore.Components;

namespace FinanceServices.ManagementPortal.Blazor.WebAssembly;

public abstract class FinanceServicesBlazorWebAppComponentBase : AspNetCoreComponentBase
{
    protected FinanceServicesBlazorWebAppComponentBase()
    {
        LocalizationResource = typeof(FinanceServicesResource);
    }
}
