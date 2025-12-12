using ByteLabs.FinanceServices.Services.FinanceServices.Permissions;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.Authorization.Permissions;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Web.Menus;

public class FinanceServicesServiceMenuContributor : IMenuContributor
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
            FinanceServicesServiceMenus.Prefix,
            context.GetLocalizer<FinanceServicesServiceResource>()[$"Menu:{FinanceServicesServiceConsts.ModuleName}"],
            icon: "fa fa-folder"
        );

        context.Menu.Items.AddIfNotContains(moduleMenu);
        return moduleMenu;
    }
        
    private static void AddMenuItemProducts(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                FinanceServicesServiceMenus.Products,
                context.GetLocalizer<FinanceServicesServiceResource>()["Menu:Products"],
                url: "/Products"
            ).RequirePermissions(FinanceServicesServicePermissions.Products.Default));

        context.Menu.Items.AddIfNotContains(parentMenu);
    }
}
