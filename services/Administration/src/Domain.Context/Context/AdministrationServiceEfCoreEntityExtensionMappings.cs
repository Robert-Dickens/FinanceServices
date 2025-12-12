using ByteLabs.Foundations.Threading;

namespace ByteLabs.FinanceServices.Services.Administration.Domain.Context;

public static class AdministrationServiceEfCoreEntityExtensionMappings
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        AdministrationServiceModuleExtensionConfigurator.Configure();

        OneTimeRunner.Run(() =>
        {
        });
    }
}
