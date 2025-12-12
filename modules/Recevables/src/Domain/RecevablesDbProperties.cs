namespace ByteLabs.FinanceServices.Recevables.Domain;

public static class RecevablesDbProperties
{
    public static string DbTablePrefix { get; set; } = "";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = RecevablesConsts.ModuleName;

}
