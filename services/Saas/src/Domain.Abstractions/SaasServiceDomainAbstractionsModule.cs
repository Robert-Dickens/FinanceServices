using ByteLabs.FinanceServices.Services.Saas.Localization;
using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.Validation.Localization;
using ByteLabs.Foundations.VirtualFileSystem;
using ByteLabs.PlatformServices.SaaS;

namespace ByteLabs.FinanceServices.Services.Saas;

[DependsOn(
    typeof(SaasDomainAbstractionsModule)
)]
public class SaasServiceDomainAbstractionsModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        SaasServiceModuleExtensionConfigurator.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<VirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<SaasServiceDomainAbstractionsModule>();
        });

        Configure<PlatformLocalizationOptions>(options =>
        {
            options.Resources
                .Add<SaasServiceResource>("en")
                .AddBaseTypes(typeof(ValidationResource))
                .AddVirtualJson("/Localization/" + GlobalConstants.Services.SaasServiceName);
        });
    }
}
