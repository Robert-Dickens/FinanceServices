using ByteLabs.FinanceServices.Services.FinanceServices.Localization;
using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.Localization.ExceptionHandling;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.Validation;
using ByteLabs.Foundations.Validation.Localization;
using ByteLabs.Foundations.VirtualFileSystem;

namespace ByteLabs.FinanceServices.Services.FinanceServices;

[DependsOn(
    typeof(ValidationModule)
)]
public class FinanceServicesServiceDomainAbstractionsModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        FinanceServicesServiceModuleExtensionConfigurator.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<VirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<FinanceServicesServiceDomainAbstractionsModule>();
        });

        Configure<PlatformLocalizationOptions>(options =>
        {
            options.Resources
                .Add<FinanceServicesServiceResource>("en")
                .AddBaseTypes(typeof(ValidationResource))
                 .AddVirtualJson("/Localization/" + FinanceServicesServiceConsts.ModuleName);

        });

        Configure<ExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(FinanceServicesServiceConsts.ModuleName, typeof(FinanceServicesServiceResource));
        });
    }
}
