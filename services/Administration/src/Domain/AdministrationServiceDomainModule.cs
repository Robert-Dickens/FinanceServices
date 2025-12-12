using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Platform.Foundations.Domain;

namespace ByteLabs.FinanceServices.Services.Administration.Domain;

[DependsOn(
    typeof(AdministrationServiceDomainAbstractionsModule),
    typeof(PlatformServicesDomainModule)
)]
public class AdministrationServiceDomainModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PlatformLocalizationOptions>(options =>
        {
            /* These languages are used on data seed. If you add new, you need to run the seed data */
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
        });
    }
}
