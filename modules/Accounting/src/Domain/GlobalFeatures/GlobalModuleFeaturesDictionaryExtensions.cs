using ByteLabs.Foundations;
using ByteLabs.Foundations.GlobalFeatures;
using JetBrains.Annotations;

namespace ByteLabs.FinanceServices.Accounting.Domain.GlobalFeatures;

public static class GlobalModuleFeaturesDictionaryExtensions
{
    public static AccountingGlobalFeature AccountingService([NotNull] this GlobalModuleFeaturesDictionary modules)
    {
        Check.NotNull(modules, nameof(modules));

        return modules
                .GetOrAdd(
                    AccountingGlobalFeature.ModuleName,
                    _ => new AccountingGlobalFeature(modules.FeatureManager)
                )
            as AccountingGlobalFeature;
    }

    public static GlobalModuleFeaturesDictionary AccountingService([NotNull] this GlobalModuleFeaturesDictionary modules,[NotNull] Action<AccountingGlobalFeature> configureAction)
    {
        Check.NotNull(configureAction, nameof(configureAction));

        configureAction(modules.AccountingService());

        return modules;
    }

}