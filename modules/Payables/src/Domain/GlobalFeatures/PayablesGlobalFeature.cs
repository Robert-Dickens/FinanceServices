using ByteLabs.FinanceServices.Payables.Domain.Features;
using ByteLabs.Foundations.GlobalFeatures;

namespace ByteLabs.FinanceServices.Payables.Domain.GlobalFeatures
{
    public class PayablesGlobalFeature : GlobalModuleFeatures
    {
        public const string ModuleName = PayablesFeatures.GroupName;

        public PayablesFeature PayablesFeature => GetFeature<PayablesFeature>();

        public PayablesGlobalFeature(GlobalFeatureManager featureManager) : base(featureManager)
        {
            AddFeature(new PayablesFeature(this));

        }
    }
}
