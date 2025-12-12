using ByteLabs.FinanceServices.Services.FinanceServices.Domain.GlobalFeatures;
using ByteLabs.FinanceServices.Services.FinanceServices.Localization;
using ByteLabs.Foundations.Features;
using ByteLabs.Foundations.GlobalFeatures;
using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.Validation.StringValues;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Domain.Features;

public class FinanceServicesServiceFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.GetOrCreateGroup(FinanceServicesFeatures.GroupName, L("Products"));

        if (GlobalFeatureManager.Instance.IsEnabled<FinanceServicesServiceFeature>())
        {
            group.AddOrUpdateFeature(FinanceServicesServiceFeature.Name,
            "true",
            L("Feature:FinanceServicesFeatures"),
            L("Feature:FinanceServicesFeaturesDescription"),
            new ToggleStringValueType());
        }
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<FinanceServicesServiceResource>(name);
    }
}
