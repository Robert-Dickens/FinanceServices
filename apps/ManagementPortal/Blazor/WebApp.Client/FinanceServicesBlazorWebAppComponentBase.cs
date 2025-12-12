using ByteLabs.Foundations.AspNetCore.Components;
using FinanceServices.Shared.Localization;

namespace FinanceServices.ManagementPortal.Blazor.WebAssembly;

public abstract class FinanceServicesBlazorWebAppComponentBase : AspNetCoreComponentBase
{
    protected FinanceServicesBlazorWebAppComponentBase()
    {
        LocalizationResource = typeof(FinanceServicesResource);
    }
}
