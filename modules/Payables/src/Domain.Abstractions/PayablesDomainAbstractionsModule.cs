using ByteLabs.FinanceServices.Payables.Localization;
using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.Localization.ExceptionHandling;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.Validation;
using ByteLabs.Foundations.Validation.Localization;
using ByteLabs.Foundations.VirtualFileSystem;

namespace ByteLabs.FinanceServices.Payables;

[DependsOn(
    typeof(ValidationModule)
)]
public class PayablesDomainAbstractionsModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PayablesModuleExtensionConfigurator.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<VirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<PayablesDomainAbstractionsModule>();
        });

        Configure<PlatformLocalizationOptions>(options =>
        {
            options.Resources
                .Add<PayablesResource>("en")
                .AddBaseTypes(typeof(ValidationResource))
                .AddVirtualJson("/Localization/" + PayablesConsts.ModuleName);
        });

        Configure<ExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(PayablesConsts.ModuleName, typeof(PayablesResource));

        });
    }
}
