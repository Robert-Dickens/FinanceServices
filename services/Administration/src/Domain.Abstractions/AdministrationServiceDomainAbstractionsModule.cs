using ByteLabs.FinanceServices.Services.Administration.Localization;
using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.Validation.Localization;
using ByteLabs.Foundations.VirtualFileSystem;
using ByteLabs.Platform.Foundations;
using FinanceServices.Shared;

namespace ByteLabs.FinanceServices.Services.Administration;

[DependsOn(
    typeof(PlatformServicesDomainAbstractionsModule)
)]
public class AdministrationServiceDomainAbstractionsModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AdministrationServiceModuleExtensionConfigurator.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<VirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AdministrationServiceDomainAbstractionsModule>();
        });

        Configure<PlatformLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AdministrationServiceResource>("en")
                .AddBaseTypes(typeof(ValidationResource))
                .AddVirtualJson("/Localization/" + GlobalConstants.Services.AdministrationServiceName);
        });
    }
}
