using ByteLabs.FinanceServices.Services.FinanceServices.Domain.Features;
using ByteLabs.Foundations.GlobalFeatures;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Domain.GlobalFeatures
{
    public class FinanceServicesServiceGlobalFeature : GlobalModuleFeatures
    {
        public const string ModuleName = FinanceServicesFeatures.GroupName;

        public FinanceServicesServiceFeature FinanceServicesFeature => GetFeature<FinanceServicesServiceFeature>();

        public FinanceServicesServiceGlobalFeature(GlobalFeatureManager featureManager) : base(featureManager)
        {
            AddFeature(new FinanceServicesServiceFeature(this));

        }
    }
}
