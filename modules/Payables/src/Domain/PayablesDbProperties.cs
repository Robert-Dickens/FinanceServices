namespace ByteLabs.FinanceServices.Payables.Domain;

public static class PayablesDbProperties
{
    public static string DbTablePrefix { get; set; } = "";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = PayablesConsts.ModuleName;

}
