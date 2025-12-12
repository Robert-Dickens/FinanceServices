using ByteLabs.FinanceServices.Accounting;
using ByteLabs.FinanceServices.Payables;
using ByteLabs.FinanceServices.Recevables;
using ByteLabs.FinanceServices.Services.FinanceServices.Localization;
using ByteLabs.Foundations.AspNetCore.Mvc;
using ByteLabs.Foundations.AspNetCore.UI.Localization;
using ByteLabs.Foundations.Localization;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.FinanceServices;

[DependsOn(
    typeof(FinanceServicesServiceApplicationAbstractionsModule),
    typeof(AspNetCoreMvcModule),
    typeof(AccountingHttpApiModule),
    typeof(PayablesHttpApiModule),
    typeof(RecevablesHttpApiModule))]
public class FinanceServicesServiceHttpApiModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(FinanceServicesServiceHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PlatformLocalizationOptions>(options =>
        {
            options.Resources
                .Get<FinanceServicesServiceResource>()
                .AddBaseTypes(typeof(UICultureResource));
        });
    }
}
