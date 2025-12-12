using ByteLabs.Foundations.AspNetCore.Mvc.UI.Layout;
using Microsoft.AspNetCore.Mvc;

namespace FinanceServices.PublicServer.Web.Themes.Bootstrap.Components.Header
{
    public class LandingHeaderViewComponent : BootstrapViewComponentBase
    {
        protected IPageLayout PageLayout { get; }

        public LandingHeaderViewComponent(IPageLayout pageLayout)
        {
            PageLayout = pageLayout;
        }

        public virtual IViewComponentResult Invoke()
        {
            return View($"~/Themes/Bootstrap/Components/Header/LandingHeader.cshtml", PageLayout.Content.MenuItemName);
        }
    }
}
