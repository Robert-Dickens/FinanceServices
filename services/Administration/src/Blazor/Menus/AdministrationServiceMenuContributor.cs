using ByteLabs.Foundations.AspNetCore.UI.Navigation;

namespace ByteLabs.FinanceServices.Services.Administration.Blazor.Menus;

public class AdministrationServiceMenuContributor : IMenuContributor
{
    public virtual async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            var moduleMenu = await ConfigureMainMenuAsync(context);
            await AddMenuItemAsync(context, moduleMenu);
        }
    }

    private Task<ApplicationMenuItem> ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        //Add main menu items.
        var administration = context.Menu.GetAdministration();
        return Task.FromResult(administration);
    }

    private Task AddMenuItemAsync(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        return Task.CompletedTask;
    }

}
