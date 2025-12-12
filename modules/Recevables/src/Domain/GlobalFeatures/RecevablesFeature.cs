using ByteLabs.Foundations.GlobalFeatures;
using JetBrains.Annotations;

namespace ByteLabs.FinanceServices.Recevables.Domain.GlobalFeatures;

[GlobalFeatureName(Name)]
public class RecevablesFeature : GlobalFeature
{
    public const string Name = RecevablesGlobalFeature.ModuleName + ".Service";

    internal RecevablesFeature([NotNull] RecevablesGlobalFeature cmsKit) : base(cmsKit)
    {
    }

    public override void Enable()
    {
        var myProjectNameFeature = FeatureManager.Modules.RecevablesService().RecevablesFeature;
        if (!myProjectNameFeature.IsEnabled)
        {
            myProjectNameFeature.Enable();
        }

        base.Enable();
    }
}
