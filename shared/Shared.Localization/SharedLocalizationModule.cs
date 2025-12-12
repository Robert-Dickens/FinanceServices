using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.Validation;
using ByteLabs.Foundations.Validation.Localization;
using ByteLabs.Foundations.VirtualFileSystem;
using FinanceServices.Shared.Localization;

namespace FinanceServices.Shared;

[DependsOn(
    typeof(ValidationModule)
    )]
public class SharedLocalizationModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<VirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<SharedLocalizationModule>();
        });

        Configure<PlatformLocalizationOptions>(options =>
        {
            options.Resources
                .Add<FinanceServicesResource>("en")
                .AddBaseTypes(
                    typeof(ValidationResource)
                ).AddVirtualJson("/Localization/" + GlobalConstants.Modules.FinanceServices);

            options.DefaultResourceType = typeof(FinanceServicesResource);
        });
    }
}
