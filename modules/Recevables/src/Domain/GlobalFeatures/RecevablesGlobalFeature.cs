using ByteLabs.FinanceServices.Recevables.Domain.Features;
using ByteLabs.Foundations.GlobalFeatures;

namespace ByteLabs.FinanceServices.Recevables.Domain.GlobalFeatures
{
    public class RecevablesGlobalFeature : GlobalModuleFeatures
    {
        public const string ModuleName = RecevablesFeatures.GroupName;

        public RecevablesFeature RecevablesFeature => GetFeature<RecevablesFeature>();

        public RecevablesGlobalFeature(GlobalFeatureManager featureManager) : base(featureManager)
        {
            AddFeature(new RecevablesFeature(this));

        }
    }
}
