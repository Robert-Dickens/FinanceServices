using ByteLabs.Foundations.Threading;

namespace ByteLabs.FinanceServices.Services.Saas.Domain.Context;

public static class SaasServiceEfCoreEntityExtensionMappings
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        SaasServiceModuleExtensionConfigurator.Configure();

        OneTimeRunner.Run(() =>
        {
        });
    }
}
