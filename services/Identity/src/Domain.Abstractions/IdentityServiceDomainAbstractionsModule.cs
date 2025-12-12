using ByteLabs.FinanceServices.Services.Identity.Localization;
using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.Validation.Localization;
using ByteLabs.Foundations.VirtualFileSystem;
using ByteLabs.PlatformServices.Identity.Domain;
using ByteLabs.PlatformServices.Security.IdentityServer;

namespace ByteLabs.FinanceServices.Services.Identity;

[DependsOn(typeof(IdentityDomainAbstractionsModule), typeof(IdentityServerDomainAbstractionsModule))]
public class IdentityServiceDomainAbstractionsModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        IdentityServiceModuleExtensionConfigurator.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<VirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<IdentityServiceDomainAbstractionsModule>();
        });

        Configure<PlatformLocalizationOptions>(options =>
        {
            options.Resources
                .Add<IdentityServiceResource>("en")
                .AddBaseTypes(typeof(ValidationResource))
                .AddVirtualJson("/Localization/" + GlobalConstants.Services.IdentityServiceName);
        });

    }
}
