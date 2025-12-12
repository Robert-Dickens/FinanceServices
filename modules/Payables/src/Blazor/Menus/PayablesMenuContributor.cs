using ByteLabs.FinanceServices.Payables.Localization;
using ByteLabs.FinanceServices.Payables.Permissions;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.AspNetCore.Web;
using ByteLabs.Foundations.Authorization.Permissions;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;

namespace ByteLabs.FinanceServices.Payables.Blazor.Menus;

public class PayablesMenuContributor : IMenuContributor
{
    public virtual async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            var moduleMenu = await ConfigureMainMenuAsync(context);
            await AddMenuItemProducts(context, moduleMenu);
        }
    }

    private Task<ApplicationMenuItem> ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        //Add main menu items.
        var moduleMenu = new ApplicationMenuItem(
                            PayablesMenus.Prefix,
                            context.GetLocalizer<PayablesResource>()[$"Menu:{PayablesConsts.ModuleName}"]
                        ).SetFluentIcon(new Size24.Book());

        context.Menu.Items.AddIfNotContains(moduleMenu);

        return Task.FromResult(moduleMenu);
    }

    private Task AddMenuItemProducts(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                PayablesMenus.Products,
                context.GetLocalizer<PayablesResource>()["Menu:Products"],
                url: "/Products"
            ).RequirePermissions(PayablesPermissions.Products.Default));

        context.Menu.Items.AddIfNotContains(parentMenu);
        return Task.CompletedTask;
    }

}
