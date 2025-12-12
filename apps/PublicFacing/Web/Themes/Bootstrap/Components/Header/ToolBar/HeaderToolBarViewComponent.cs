using ByteLabs.Foundations.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Microsoft.AspNetCore.Mvc;

namespace FinanceServices.PublicServer.Web.Themes.Bootstrap.Components.Header.ToolBar
{
    public class HeaderToolBarViewComponent : BootstrapViewComponentBase
    {
        protected IToolbarManager ToolbarManager { get; }

        public HeaderToolBarViewComponent(IToolbarManager toolbarManager)
        {
            ToolbarManager = toolbarManager;
        }

        public virtual async Task<IViewComponentResult> InvokeAsync()
        {
            var toolbar = await ToolbarManager.GetAsync(StandardToolbars.Main);
            return View($"~/Themes/Bootstrap/Components/Header/ToolBar/Default.cshtml", toolbar);
        }
    }
}
