using ByteLabs.Foundations.AspNetCore.UI.Branding;
using ByteLabs.Foundations.DependencyInjection;

namespace FinanceServices.ManagementPortal.Web;

[Dependency(ReplaceServices = true)]
public class FinanceServicesBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "FinanceServices";
}
