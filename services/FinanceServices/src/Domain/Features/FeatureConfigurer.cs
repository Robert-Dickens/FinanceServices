using ByteLabs.FinanceServices.Services.FinanceServices.Domain.GlobalFeatures;
using ByteLabs.Foundations.GlobalFeatures;
using ByteLabs.Foundations.Threading;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Domain.Features;

public static class FeatureConfigurer
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void EnableAll()
    {
        OneTimeRunner.Run(() =>
        {
            GlobalFeatureManager.Instance.Modules.FinanceServicesService().EnableAll();
        });
    }
}