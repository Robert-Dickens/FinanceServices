using ByteLabs.FinanceServices.Payables.Localization;
using ByteLabs.Foundations.AspNetCore.Mvc;
using ByteLabs.Foundations.AspNetCore.UI.Localization;
using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Payables;

[DependsOn(
    typeof(PayablesApplicationAbstractionsModule),
    typeof(AspNetCoreMvcModule))]
public class PayablesHttpApiModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(PayablesHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PlatformLocalizationOptions>(options =>
        {
            options.Resources
                .Get<PayablesResource>()
                .AddBaseTypes(typeof(UICultureResource));
        });
    }
}
