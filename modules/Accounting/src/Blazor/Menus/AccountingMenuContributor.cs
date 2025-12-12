using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.AspNetCore.Web;
using ByteLabs.Foundations.Authorization.Permissions;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;
using ByteLabs.FinanceServices.Accounting.Localization;
using ByteLabs.FinanceServices.Accounting.Permissions;

namespace ByteLabs.FinanceServices.Accounting.Blazor.Menus;

public class AccountingMenuContributor : IMenuContributor
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
                            AccountingMenus.Prefix,
                            context.GetLocalizer<AccountingResource>()[$"Menu:{AccountingConsts.ModuleName}"]
                        ).SetFluentIcon(new Size24.Book());

        context.Menu.Items.AddIfNotContains(moduleMenu);

        return Task.FromResult(moduleMenu);
    }

    private Task AddMenuItemProducts(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                AccountingMenus.Products,
                context.GetLocalizer<AccountingResource>()["Menu:Products"],
                url: "/Products"
            ).RequirePermissions(AccountingPermissions.Products.Default));

        context.Menu.Items.AddIfNotContains(parentMenu);
        return Task.CompletedTask;
    }

}
