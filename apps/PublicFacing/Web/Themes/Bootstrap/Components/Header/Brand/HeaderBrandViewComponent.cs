using Microsoft.AspNetCore.Mvc;

namespace FinanceServices.PublicServer.Web.Themes.Bootstrap.Components.Header.Brand
{
    public class HeaderBrandViewComponent : BootstrapViewComponentBase
    {
        public virtual IViewComponentResult Invoke()
        {
            return View($"~/Themes/Bootstrap/Components/Header/Brand/Default.cshtml");
        }
    }
}
