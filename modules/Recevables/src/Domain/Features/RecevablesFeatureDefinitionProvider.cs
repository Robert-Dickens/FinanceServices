using ByteLabs.FinanceServices.Recevables.Domain.GlobalFeatures;
using ByteLabs.FinanceServices.Recevables.Localization;
using ByteLabs.Foundations.Features;
using ByteLabs.Foundations.GlobalFeatures;
using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.Validation.StringValues;

namespace ByteLabs.FinanceServices.Recevables.Domain.Features;

public class RecevablesFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.GetOrCreateGroup(RecevablesFeatures.GroupName, L("Products"));

        if (GlobalFeatureManager.Instance.IsEnabled<RecevablesFeature>())
        {
            group.AddOrUpdateFeature(RecevablesFeature.Name,
            "true",
            L("Feature:RecevablesFeatures"),
            L("Feature:RecevablesFeaturesDescription"),
            new ToggleStringValueType());
        }
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<RecevablesResource>(name);
    }
}
