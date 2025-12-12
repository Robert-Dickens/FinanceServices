using ByteLabs.Foundations.Threading;

namespace ByteLabs.FinanceServices.Services.Identity;

public static class IdentityServiceModuleExtensionConfigurator
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        OneTimeRunner.Run(() =>
        {
            ConfigureExistingProperties();
            ConfigureExtraProperties();
        });
    }

    private static void ConfigureExistingProperties()
    {
    }

    private static void ConfigureExtraProperties()
    {
    }
}
