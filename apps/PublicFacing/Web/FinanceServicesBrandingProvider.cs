using ByteLabs.FinanceServices;
using ByteLabs.Foundations.AspNetCore.UI.Branding;
using ByteLabs.Foundations.DependencyInjection;

namespace FinanceServices.PublicServer.Web;

[Dependency(ReplaceServices = true)]
public class FinanceServicesBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => GlobalConstants.Clients.PublicPortalServiceName;
}
