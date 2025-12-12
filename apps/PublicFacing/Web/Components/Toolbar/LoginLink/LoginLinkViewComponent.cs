using ByteLabs.Foundations.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace FinanceServices.PublicServer.Web.Components.Toolbar.LoginLink;

public class LoginLinkViewComponent : AspNetCoreViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Components/Toolbar/LoginLink/Default.cshtml");
    }
}
