using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.Authorization.Permissions;
using ByteLabs.FinanceServices.Accounting.Localization;
using ByteLabs.FinanceServices.Accounting.Permissions;

namespace ByteLabs.FinanceServices.Accounting.Web.Menus;

public class AccountingMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return;
        }

        var moduleMenu = AddModuleMenuItem(context);
        AddMenuItemProducts(context, moduleMenu);
    }

    private static ApplicationMenuItem AddModuleMenuItem(MenuConfigurationContext context)
    {
        var moduleMenu = new ApplicationMenuItem(
            AccountingMenus.Prefix,
            context.GetLocalizer<AccountingResource>()[$"Menu:{AccountingConsts.ModuleName}"],
            icon: "fa fa-folder"
        );

        context.Menu.Items.AddIfNotContains(moduleMenu);
        return moduleMenu;
    }
        
    private static void AddMenuItemProducts(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                AccountingMenus.Products,
                context.GetLocalizer<AccountingResource>()["Menu:Products"],
                url: "/Products"
            ).RequirePermissions(AccountingPermissions.Products.Default));

        context.Menu.Items.AddIfNotContains(parentMenu);
    }
}
