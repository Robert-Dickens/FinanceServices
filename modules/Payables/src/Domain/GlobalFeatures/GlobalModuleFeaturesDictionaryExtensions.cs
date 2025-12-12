using ByteLabs.Foundations;
using ByteLabs.Foundations.GlobalFeatures;
using JetBrains.Annotations;

namespace ByteLabs.FinanceServices.Payables.Domain.GlobalFeatures;

public static class GlobalModuleFeaturesDictionaryExtensions
{
    public static PayablesGlobalFeature PayablesService([NotNull] this GlobalModuleFeaturesDictionary modules)
    {
        Check.NotNull(modules, nameof(modules));

        return modules
                .GetOrAdd(
                    PayablesGlobalFeature.ModuleName,
                    _ => new PayablesGlobalFeature(modules.FeatureManager)
                )
            as PayablesGlobalFeature;
    }

    public static GlobalModuleFeaturesDictionary PayablesService([NotNull] this GlobalModuleFeaturesDictionary modules,[NotNull] Action<PayablesGlobalFeature> configureAction)
    {
        Check.NotNull(configureAction, nameof(configureAction));

        configureAction(modules.PayablesService());

        return modules;
    }

}