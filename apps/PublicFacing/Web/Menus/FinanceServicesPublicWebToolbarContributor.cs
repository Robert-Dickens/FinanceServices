using ByteLabs.Foundations.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using ByteLabs.Foundations.Users;
using FinanceServices.PublicServer.Web.Components.Toolbar.LoginLink;

namespace FinanceServices.PublicServer.Web.Menus;

public class FinanceServicesPublicWebToolbarContributor : IToolbarContributor
{
    public virtual Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name != StandardToolbars.Main)
        {
            return Task.CompletedTask;
        }

        if (!context.ServiceProvider.GetRequiredService<ICurrentUser>().IsAuthenticated)
        {
            context.Toolbar.Items.Add(new ToolbarItem(typeof(LoginLinkViewComponent)));
        }

        return Task.CompletedTask;
    }
}
