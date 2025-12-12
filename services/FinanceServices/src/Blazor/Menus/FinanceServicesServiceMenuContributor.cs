using ByteLabs.FinanceServices.Services.FinanceServices.Localization;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.Authorization.Permissions;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Blazor.Menus;

public class FinanceServicesServiceMenuContributor : IMenuContributor
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
                            FinanceServicesServiceMenus.Prefix,
                            context.GetLocalizer<FinanceServicesServiceResource>()[$"Menu:{FinanceServicesServiceConsts.ModuleName}"],
                            icon: "fa fa-folder"
                        );

        context.Menu.Items.AddIfNotContains(moduleMenu);

        return Task.FromResult(moduleMenu);
    }

    private Task AddMenuItemProducts(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            PermissionSimpleStateCheckerExtensions.RequirePermissions(new ApplicationMenuItem(
                FinanceServicesServiceMenus.Products,
                context.GetLocalizer<FinanceServicesServiceResource>()["Menu:Products"],
                url: "/Products"
            ), [FinanceServicesServicePermissions.Products.Default]));

        context.Menu.Items.AddIfNotContains(parentMenu);
        return Task.CompletedTask;
    }

}
