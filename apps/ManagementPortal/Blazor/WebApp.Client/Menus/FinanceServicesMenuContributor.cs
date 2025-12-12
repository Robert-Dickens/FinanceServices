using ByteLabs.FinanceServices.Localization.Localization;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.Authorization.Permissions;

namespace FinanceServices.ManagementPortal.Blazor.WebAssembly.Menus;

public class FinanceServicesMenuContributor : IMenuContributor
{
    private readonly IConfiguration _configuration;

    public FinanceServicesMenuContributor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
        else if (context.Menu.Name == StandardMenus.User)
        {
            await ConfigureUserMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<FinanceServicesResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                FinanceServicesMenus.Home,
                l["Menu:Home"],
                "/",
                icon: "fas fa-home"
            )
        );

        context.Menu.GetAdministration();


        foreach(var itm in context.Menu.Items)
        {
            if (itm.Icon.IsNullOrEmpty())
            {
                itm.Icon = itm.Icon.RemovePreFix("fas").Trim();
            }
        }


        return Task.CompletedTask;
    }

    private Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        if (!OperatingSystem.IsBrowser())
        {
            return Task.CompletedTask;
        }

        var authServerUrl = _configuration["AuthServer:Authority"] ?? "";
        var accountStringLocalizer = context.GetLocalizer<FinanceServicesResource>();

        context.Menu.AddItem(new ApplicationMenuItem(
                "Account.Manage",
                accountStringLocalizer["MyAccount"],
                $"{authServerUrl.EnsureEndsWith('/')}Account/Manage",
                icon: "fa-cog",
                order: 1000,
                target: "_blank")
            .RequireAuthenticated());

        return Task.CompletedTask;
    }
}
