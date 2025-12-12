using FinanceServices.Shared;

namespace ByteLabs.FinanceServices.Services.Identity.Domain;

public static class IdentityServiceDbProperties
{
    public static string DbTablePrefix { get; set; } = "";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = GlobalConstants.Databases.IdentityServiceConnectionStringName;

    public const string DefaultAdminEmailAddress = GlobalConstants.Identity.DefaultAdminEmailAddress;

    public const string DefaultAdminPassword = GlobalConstants.Identity.DefaultAdminPassword;
}
