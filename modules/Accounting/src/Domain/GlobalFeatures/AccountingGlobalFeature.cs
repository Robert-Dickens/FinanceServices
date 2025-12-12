using ByteLabs.FinanceServices.Accounting.Domain.Features;
using ByteLabs.Foundations.GlobalFeatures;

namespace ByteLabs.FinanceServices.Accounting.Domain.GlobalFeatures
{
    public class AccountingGlobalFeature : GlobalModuleFeatures
    {
        public const string ModuleName = AccountingFeatures.GroupName;

        public AccountingFeature AccountingFeature => GetFeature<AccountingFeature>();

        public AccountingGlobalFeature(GlobalFeatureManager featureManager) : base(featureManager)
        {
            AddFeature(new AccountingFeature(this));

        }
    }
}
