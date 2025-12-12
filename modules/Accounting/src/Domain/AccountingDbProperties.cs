namespace ByteLabs.FinanceServices.Accounting.Domain;

public static class AccountingDbProperties
{
    public static string DbTablePrefix { get; set; } = "";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = AccountingConsts.ModuleName;

}
