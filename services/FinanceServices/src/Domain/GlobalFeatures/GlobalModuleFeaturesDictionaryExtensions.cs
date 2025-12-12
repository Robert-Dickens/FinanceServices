using ByteLabs.Foundations;
using ByteLabs.Foundations.GlobalFeatures;
using JetBrains.Annotations;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Domain.GlobalFeatures;

public static class GlobalModuleFeaturesDictionaryExtensions
{
    public static FinanceServicesServiceGlobalFeature FinanceServicesService([NotNull] this GlobalModuleFeaturesDictionary modules)
    {
        Check.NotNull(modules, nameof(modules));

        return modules
                .GetOrAdd(
                    FinanceServicesServiceGlobalFeature.ModuleName,
                    _ => new FinanceServicesServiceGlobalFeature(modules.FeatureManager)
                )
            as FinanceServicesServiceGlobalFeature;
    }

    public static GlobalModuleFeaturesDictionary FinanceServicesService([NotNull] this GlobalModuleFeaturesDictionary modules,[NotNull] Action<FinanceServicesServiceGlobalFeature> configureAction)
    {
        Check.NotNull(configureAction, nameof(configureAction));

        configureAction(modules.FinanceServicesService());

        return modules;
    }

}