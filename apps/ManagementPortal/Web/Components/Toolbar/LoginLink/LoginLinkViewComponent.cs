using Microsoft.AspNetCore.Mvc;
using ByteLabs.Foundations.AspNetCore.Mvc;

namespace FinanceServices.ManagementPortal.Web.Components.Toolbar.LoginLink;

public class LoginLinkViewComponent : AspNetCoreViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Components/Toolbar/LoginLink/Default.cshtml");
    }
}
