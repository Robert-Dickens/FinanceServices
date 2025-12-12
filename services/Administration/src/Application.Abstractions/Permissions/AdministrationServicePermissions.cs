using FinanceServices.Shared;

namespace ByteLabs.FinanceServices.Services.Administration.Permissions;

public static class AdministrationServicePermissions
{
    public const string GroupName = GlobalConstants.Services.AdministrationServiceName;

    public static class Dashboard
    {
        public const string DashboardGroup = GroupName + ".Dashboard";
        public const string Host = DashboardGroup + ".Host";
        public const string Tenant = DashboardGroup + ".Tenant";
    }

    public static class Settings
    {
        public const string SettingsGroup = GroupName + ".Settings";
        public const string Default = SettingsGroup;
        public const string Host = SettingsGroup + ".Host";
        public const string Tenant = SettingsGroup + ".Tenant";
    }

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";
}
