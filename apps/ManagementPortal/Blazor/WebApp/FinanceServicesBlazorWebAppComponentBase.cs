using ByteLabs.Foundations.AspNetCore.Components;
using FinanceServices.Shared.Localization;

namespace FinanceServices.ManagementPortal.Blazor;

public abstract class FinanceServicesBlazorWebAppComponentBase : AspNetCoreComponentBase
{
    protected FinanceServicesBlazorWebAppComponentBase()
    {
        LocalizationResource = typeof(FinanceServicesResource);
    }
}
