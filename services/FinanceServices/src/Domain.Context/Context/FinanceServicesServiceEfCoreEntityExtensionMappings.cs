using ByteLabs.Foundations.Threading;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Domain.Context;

public static class FinanceServicesServiceEfCoreEntityExtensionMappings
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        FinanceServicesServiceModuleExtensionConfigurator.Configure();

        OneTimeRunner.Run(() =>
        {
        });
    }
}
