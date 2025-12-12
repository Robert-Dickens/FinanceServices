using ByteLabs.FinanceServices.Localization.Localization;
using ByteLabs.Foundations.AspNetCore.UI.Localization;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.Authorization.Permissions;
using ByteLabs.PlatformServices.Account.Localization;

namespace FinanceServices.PublicServer.Web.Menus;

public class FinanceServicesPublicWebMenuContributor : IMenuContributor
{
    private readonly IConfiguration _configuration;

    public FinanceServicesPublicWebMenuContributor(IConfiguration configuration)
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

        // Home
        context.Menu.AddItem(
            new ApplicationMenuItem(
                FinanceServicesPublicWebMenus.HomePage,
                l["Menu:Home"],
                "~/",
                icon: "fa fa-home",
                order: 0
            )
        );
        // Products
        context.Menu.AddItem(
            new ApplicationMenuItem(
                FinanceServicesPublicWebMenus.ProductPage,
                "Products",
                "/Products",
                icon: "fa fa-product-hunt",
                order: 1
            )
        );

        return Task.CompletedTask;
    }

    private Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        var identityServerUrl = _configuration["AuthServer:Authority"] ?? "~";
        var uiResource = context.GetLocalizer<UICultureResource>();
        var accountResource = context.GetLocalizer<AccountResource>();
        context.Menu.AddItem(new ApplicationMenuItem("Account.Manage", accountResource["MyAccount"], $"{identityServerUrl.EnsureEndsWith('/')}Account/Manage", icon: "fa fa-cog", order: 1000, null, "_blank").RequireAuthenticated());
        context.Menu.AddItem(new ApplicationMenuItem("Account.SecurityLogs", accountResource["MySecurityLogs"], $"{identityServerUrl.EnsureEndsWith('/')}Account/SecurityLogs", target: "_blank").RequireAuthenticated());
        context.Menu.AddItem(new ApplicationMenuItem("Account.Logout", uiResource["Logout"], url: "~/Account/Logout", icon: "fa fa-power-off", order: int.MaxValue - 1000).RequireAuthenticated());

        return Task.CompletedTask;
    }
}
