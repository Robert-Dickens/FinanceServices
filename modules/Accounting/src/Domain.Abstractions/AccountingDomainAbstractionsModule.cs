using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.Localization.ExceptionHandling;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.VirtualFileSystem;
using ByteLabs.FinanceServices.Accounting.Localization;
using ByteLabs.Foundations.Validation.Localization;
using ByteLabs.Foundations.Validation;

namespace ByteLabs.FinanceServices.Accounting;

[DependsOn(
    typeof(ValidationModule)
)]
public class AccountingDomainAbstractionsModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AccountingModuleExtensionConfigurator.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<VirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AccountingDomainAbstractionsModule>();
        });

        Configure<PlatformLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AccountingResource>("en")
                .AddBaseTypes(typeof(ValidationResource))
                .AddVirtualJson("/Localization/" + AccountingConsts.ModuleName);
        });

        Configure<ExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(AccountingConsts.ModuleName, typeof(AccountingResource));

        });
    }
}
