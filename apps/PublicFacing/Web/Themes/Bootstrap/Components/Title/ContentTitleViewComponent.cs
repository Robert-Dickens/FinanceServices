using ByteLabs.Foundations.AspNetCore.Mvc.UI.Layout;
using Microsoft.AspNetCore.Mvc;

namespace FinanceServices.PublicServer.Web.Themes.Bootstrap.Components.Title
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
            return View($"~/Themes/Bootstrap/Components/Title/Default.cshtml", PageLayout.Content.Title);
        }
    }
}
