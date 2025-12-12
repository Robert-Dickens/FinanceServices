using ByteLabs.Foundations.GlobalFeatures;
using JetBrains.Annotations;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Domain.GlobalFeatures;

[GlobalFeatureName(Name)]
public class FinanceServicesServiceFeature : GlobalFeature
{
    public const string Name = FinanceServicesServiceGlobalFeature.ModuleName + ".Service";

    internal FinanceServicesServiceFeature([NotNull] FinanceServicesServiceGlobalFeature cmsKit) : base(cmsKit)
    {
    }

    public override void Enable()
    {
        var myProjectNameFeature = FeatureManager.Modules.FinanceServicesService().FinanceServicesFeature;
        if (!myProjectNameFeature.IsEnabled)
        {
            myProjectNameFeature.Enable();
        }

        base.Enable();
    }
}
