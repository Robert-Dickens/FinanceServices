using ByteLabs.FinanceServices;
using ByteLabs.Foundations.AspNetCore.UI.Branding;
using ByteLabs.Foundations.DependencyInjection;

namespace FinanceServices.ManagementPortal.Blazor.WebAssembly;

[Dependency(ReplaceServices = true)]
public class FinanceServicesBlazorWebAppBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => GlobalConstants.Clients.BlazorManagementPortalServiceName;
}
