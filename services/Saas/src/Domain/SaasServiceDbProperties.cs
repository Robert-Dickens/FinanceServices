namespace ByteLabs.FinanceServices.Services.Saas.Domain;

public static class SaasServiceDbProperties
{
    public static string DbTablePrefix { get; set; } = "";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = GlobalConstants.Databases.SaasServiceConnectionStringName;
}
