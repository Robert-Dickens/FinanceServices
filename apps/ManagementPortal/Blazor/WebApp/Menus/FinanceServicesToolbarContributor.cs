using ByteLabs.Foundations.AspNetCore.Components.Web.Toolbars;

namespace FinanceServices.ManagementPortal.Blazor.Menus;

public class FinanceServicesToolbarContributor : IToolbarContributor
{
    public virtual Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name != StandardToolbars.Main)
        {
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}
