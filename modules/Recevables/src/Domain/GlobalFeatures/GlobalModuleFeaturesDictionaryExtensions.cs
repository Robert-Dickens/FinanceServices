using ByteLabs.Foundations;
using ByteLabs.Foundations.GlobalFeatures;
using JetBrains.Annotations;

namespace ByteLabs.FinanceServices.Recevables.Domain.GlobalFeatures;

public static class GlobalModuleFeaturesDictionaryExtensions
{
    public static RecevablesGlobalFeature RecevablesService([NotNull] this GlobalModuleFeaturesDictionary modules)
    {
        Check.NotNull(modules, nameof(modules));

        return modules
                .GetOrAdd(
                    RecevablesGlobalFeature.ModuleName,
                    _ => new RecevablesGlobalFeature(modules.FeatureManager)
                )
            as RecevablesGlobalFeature;
    }

    public static GlobalModuleFeaturesDictionary RecevablesService([NotNull] this GlobalModuleFeaturesDictionary modules,[NotNull] Action<RecevablesGlobalFeature> configureAction)
    {
        Check.NotNull(configureAction, nameof(configureAction));

        configureAction(modules.RecevablesService());

        return modules;
    }

}