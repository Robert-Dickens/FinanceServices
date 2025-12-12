using ByteLabs.FinanceServices.Payables.Localization;
using ByteLabs.FinanceServices.Payables.Permissions;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.Authorization.Permissions;

namespace ByteLabs.FinanceServices.Payables.Web.Menus;

public class PayablesMenuContributor : IMenuContributor
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
            PayablesMenus.Prefix,
            context.GetLocalizer<PayablesResource>()[$"Menu:{PayablesConsts.ModuleName}"],
            icon: "fa fa-folder"
        );

        context.Menu.Items.AddIfNotContains(moduleMenu);
        return moduleMenu;
    }
        
    private static void AddMenuItemProducts(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                PayablesMenus.Products,
                context.GetLocalizer<PayablesResource>()["Menu:Products"],
                url: "/Products"
            ).RequirePermissions(PayablesPermissions.Products.Default));

        context.Menu.Items.AddIfNotContains(parentMenu);
    }
}
