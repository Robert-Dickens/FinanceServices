using ByteLabs.FinanceServices.Payables.Domain.GlobalFeatures;
using ByteLabs.Foundations.GlobalFeatures;
using ByteLabs.Foundations.Threading;

namespace ByteLabs.FinanceServices.Payables.Domain.Features;

public static class FeatureConfigurer
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void EnableAll()
    {
        OneTimeRunner.Run(() =>
        {
            GlobalFeatureManager.Instance.Modules.PayablesService().EnableAll();
        });
    }
}