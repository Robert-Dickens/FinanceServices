using ByteLabs.FinanceServices.Recevables.Localization;
using ByteLabs.FinanceServices.Recevables.Permissions;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.Authorization.Permissions;

namespace ByteLabs.FinanceServices.Recevables.Web.Menus;

public class RecevablesMenuContributor : IMenuContributor
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
            RecevablesMenus.Prefix,
            context.GetLocalizer<RecevablesResource>()[$"Menu:{RecevablesConsts.ModuleName}"],
            icon: "fa fa-folder"
        );

        context.Menu.Items.AddIfNotContains(moduleMenu);
        return moduleMenu;
    }
        
    private static void AddMenuItemProducts(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                RecevablesMenus.Products,
                context.GetLocalizer<RecevablesResource>()["Menu:Products"],
                url: "/Products"
            ).RequirePermissions(RecevablesPermissions.Products.Default));

        context.Menu.Items.AddIfNotContains(parentMenu);
    }
}
