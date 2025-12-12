using ByteLabs.Foundations.AspNetCore.Mvc;
using ByteLabs.Foundations.AspNetCore.UI.Localization;
using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;
using ByteLabs.FinanceServices.Accounting.Localization;

namespace ByteLabs.FinanceServices.Accounting;

[DependsOn(
    typeof(AccountingApplicationAbstractionsModule),
    typeof(AspNetCoreMvcModule))]
public class AccountingHttpApiModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AccountingHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PlatformLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AccountingResource>()
                .AddBaseTypes(typeof(UICultureResource));
        });
    }
}
