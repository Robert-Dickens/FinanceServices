namespace ByteLabs.FinanceServices.Services.FinanceServices.Domain;

public static class FinanceServicesServiceDbProperties
{
    public static string DbTablePrefix { get; set; } = "";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = FinanceServicesServiceConsts.ModuleName;

}
