using ByteLabs.FinanceServices.Recevables.Localization;
using ByteLabs.Foundations.AspNetCore.Mvc;
using ByteLabs.Foundations.AspNetCore.UI.Localization;
using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Recevables;

[DependsOn(
    typeof(RecevablesApplicationAbstractionsModule),
    typeof(AspNetCoreMvcModule))]
public class RecevablesHttpApiModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(RecevablesHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PlatformLocalizationOptions>(options =>
        {
            options.Resources
                .Get<RecevablesResource>()
                .AddBaseTypes(typeof(UICultureResource));
        });
    }
}
