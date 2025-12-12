using ByteLabs.FinanceServices.Payables.Domain.GlobalFeatures;
using ByteLabs.FinanceServices.Payables.Localization;
using ByteLabs.Foundations.Features;
using ByteLabs.Foundations.GlobalFeatures;
using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.Validation.StringValues;

namespace ByteLabs.FinanceServices.Payables.Domain.Features;

public class PayablesFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.GetOrCreateGroup(PayablesFeatures.GroupName, L("Products"));

        if (GlobalFeatureManager.Instance.IsEnabled<PayablesFeature>())
        {
            group.AddOrUpdateFeature(PayablesFeature.Name,
            "true",
            L("Feature:PayablesFeatures"),
            L("Feature:PayablesFeaturesDescription"),
            new ToggleStringValueType());
        }
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<PayablesResource>(name);
    }
}
