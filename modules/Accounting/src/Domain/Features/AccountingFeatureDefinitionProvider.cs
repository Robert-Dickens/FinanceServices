using ByteLabs.FinanceServices.Accounting.Domain.GlobalFeatures;
using ByteLabs.FinanceServices.Accounting.Localization;
using ByteLabs.Foundations.Features;
using ByteLabs.Foundations.GlobalFeatures;
using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.Validation.StringValues;

namespace ByteLabs.FinanceServices.Accounting.Domain.Features;

public class AccountingFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.GetOrCreateGroup(AccountingFeatures.GroupName, L("Products"));

        if (GlobalFeatureManager.Instance.IsEnabled<AccountingFeature>())
        {
            group.AddOrUpdateFeature(AccountingFeature.Name,
            "true",
            L("Feature:AccountingFeatures"),
            L("Feature:AccountingFeaturesDescription"),
            new ToggleStringValueType());
        }
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AccountingResource>(name);
    }
}
