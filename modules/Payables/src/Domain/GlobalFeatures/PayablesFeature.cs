using ByteLabs.Foundations.GlobalFeatures;
using JetBrains.Annotations;

namespace ByteLabs.FinanceServices.Payables.Domain.GlobalFeatures;

[GlobalFeatureName(Name)]
public class PayablesFeature : GlobalFeature
{
    public const string Name = PayablesGlobalFeature.ModuleName + ".Service";

    internal PayablesFeature([NotNull] PayablesGlobalFeature cmsKit) : base(cmsKit)
    {
    }

    public override void Enable()
    {
        var myProjectNameFeature = FeatureManager.Modules.PayablesService().PayablesFeature;
        if (!myProjectNameFeature.IsEnabled)
        {
            myProjectNameFeature.Enable();
        }

        base.Enable();
    }
}
