using ByteLabs.FinanceServices.Recevables.Localization;
using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.Localization.ExceptionHandling;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.Validation;
using ByteLabs.Foundations.Validation.Localization;
using ByteLabs.Foundations.VirtualFileSystem;

namespace ByteLabs.FinanceServices.Recevables;

[DependsOn(
    typeof(ValidationModule)
)]
public class RecevablesDomainAbstractionsModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        RecevablesModuleExtensionConfigurator.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<VirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<RecevablesDomainAbstractionsModule>();
        });

        Configure<PlatformLocalizationOptions>(options =>
        {
            options.Resources
                .Add<RecevablesResource>("en")
                .AddBaseTypes(typeof(ValidationResource))
                .AddVirtualJson("/Localization/" + RecevablesConsts.ModuleName);
        });

        Configure<ExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(RecevablesConsts.ModuleName, typeof(RecevablesResource));

        });
    }
}
