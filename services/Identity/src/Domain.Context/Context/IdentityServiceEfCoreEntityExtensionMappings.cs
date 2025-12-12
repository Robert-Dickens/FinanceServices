using ByteLabs.Foundations.Threading;

namespace ByteLabs.FinanceServices.Services.Identity.Domain.Context;

public static class IdentityServiceEfCoreEntityExtensionMappings
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        IdentityServiceModuleExtensionConfigurator.Configure();

        OneTimeRunner.Run(() =>
        {
        });
    }
}
