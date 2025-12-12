using ByteLabs.FinanceServices.Recevables.Localization;
using ByteLabs.FinanceServices.Recevables.Permissions;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.AspNetCore.Web;
using ByteLabs.Foundations.Authorization.Permissions;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;

namespace ByteLabs.FinanceServices.Recevables.Blazor.Menus;

public class RecevablesMenuContributor : IMenuContributor
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
                            RecevablesMenus.Prefix,
                            context.GetLocalizer<RecevablesResource>()[$"Menu:{RecevablesConsts.ModuleName}"]
                        ).SetFluentIcon(new Size24.Book());

        context.Menu.Items.AddIfNotContains(moduleMenu);

        return Task.FromResult(moduleMenu);
    }

    private Task AddMenuItemProducts(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                RecevablesMenus.Products,
                context.GetLocalizer<RecevablesResource>()["Menu:Products"],
                url: "/Products"
            ).RequirePermissions(RecevablesPermissions.Products.Default));

        context.Menu.Items.AddIfNotContains(parentMenu);
        return Task.CompletedTask;
    }

}
