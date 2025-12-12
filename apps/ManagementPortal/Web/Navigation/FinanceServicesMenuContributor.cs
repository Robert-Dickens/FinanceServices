using ByteLabs.FinanceServices.Services.Administration.Permissions;
using ByteLabs.FinanceServices.Services.FinanceServices.Web.Menus;
using ByteLabs.Foundations.AspNetCore.UI.Localization;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.Authorization.Permissions;
using ByteLabs.PlatformServices.Account.Localization;
using ByteLabs.PlatformServices.AuditLogging.Web.Menus;
using ByteLabs.PlatformServices.Identity.Web.Menus;
using ByteLabs.PlatformServices.SaaS.Web.Menus;
using ByteLabs.PlatformServices.Security.IdentityServer.Web.Menus;
using ByteLabs.PlatformServices.Settings.Web.Navigation;
using FinanceServices.Shared.Localization;

namespace FinanceServices.ManagementPortal.Web.Navigation;

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

    private static async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<FinanceServicesResource>();

        //Home
        context.Menu.AddItem(
            new ApplicationMenuItem(
                FinanceServicesMenus.Home,
                l["Menu:Home"],
                "~/",
                icon: "fa fa-home",
                order: 0
            )
        );

        //Host Dashboard
        context.Menu.AddItem(
            new ApplicationMenuItem(
                FinanceServicesMenus.HostDashboard,
                l["Menu:Dashboard"],
                "~/HostDashboard",
                icon: "fa fa-line-chart",
                order: 1
            ).RequirePermissions(AdministrationServicePermissions.Dashboard.Host)
        );

        //Tenant Dashboard
        context.Menu.AddItem(
            new ApplicationMenuItem(
                FinanceServicesMenus.TenantDashboard,
                l["Menu:Dashboard"],
                "~/Dashboard",
                icon: "fa fa-line-chart",
                order: 1
            ).RequirePermissions(AdministrationServicePermissions.Dashboard.Tenant)
        );

        context.Menu.SetSubItemOrder(FinanceServicesServiceMenus.ProductManagement, 2);

        context.Menu.SetSubItemOrder(AbpSaasMenuNames.GroupName, 3);

        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 4;

        //Administration->Identity
        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 1);

        //Administration->Identity Server
        administration.SetSubItemOrder(IdentityServerManagementMenuNames.GroupName, 2);

        //Administration->Audit Logs
        administration.SetSubItemOrder(AuditLoggingMainMenuNames.GroupName, 4);

        //Administration->Settings
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 5);
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
