using ByteLabs.FinanceServices.Services.Administration.Localization;
using ByteLabs.Foundations.Authorization.Permissions;
using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.MultiTenancy;

namespace ByteLabs.FinanceServices.Services.Administration.Permissions;

public class AdministrationServicePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var administrationServiceGroup = context.AddGroup(AdministrationServicePermissions.GroupName);

        administrationServiceGroup.AddPermission(AdministrationServicePermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
        administrationServiceGroup.AddPermission(AdministrationServicePermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

        administrationServiceGroup.AddPermission(AdministrationServicePermissions.Settings.Host, L("Permission:Settings"), MultiTenancySides.Host);
        administrationServiceGroup.AddPermission(AdministrationServicePermissions.Settings.Tenant, L("Permission:Settings"), MultiTenancySides.Tenant);

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AdministrationServiceResource>(name);
    }
}
