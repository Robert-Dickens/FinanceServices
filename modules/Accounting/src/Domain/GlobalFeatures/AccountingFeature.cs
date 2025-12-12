using ByteLabs.Foundations.GlobalFeatures;
using JetBrains.Annotations;

namespace ByteLabs.FinanceServices.Accounting.Domain.GlobalFeatures;

[GlobalFeatureName(Name)]
public class AccountingFeature : GlobalFeature
{
    public const string Name = AccountingGlobalFeature.ModuleName + ".Service";

    internal AccountingFeature([NotNull] AccountingGlobalFeature cmsKit) : base(cmsKit)
    {
    }

    public override void Enable()
    {
        var myProjectNameFeature = FeatureManager.Modules.AccountingService().AccountingFeature;
        if (!myProjectNameFeature.IsEnabled)
        {
            myProjectNameFeature.Enable();
        }

        base.Enable();
    }
}
