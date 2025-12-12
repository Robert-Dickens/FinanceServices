using ByteLabs.Foundations.AspNetCore.Components;
using ByteLabs.Foundations.AspNetCore.Components.DependencyInjection;
using ByteLabs.Foundations.Modularity;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Playwright;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Testing;

[DependsOn(
    typeof(AspNetCoreComponentsModule),
    typeof(FinanceServicesServiceDomainContextTestModule),
    typeof(FinanceServicesServiceBlazorModule)
)]
public class FinanceServicesServiceAcceptanceTestModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Replace(ServiceDescriptor.Transient<IComponentActivator, ServiceProviderComponentActivator>());

        RegisterPlaywright(context);
    }

    private  void RegisterPlaywright( ServiceConfigurationContext context)
    {
        context.Services.AddTransient(async _ =>
        {
            var playwright = await Playwright.CreateAsync().ConfigureAwait(false);
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 200
            }).ConfigureAwait(false);
            return await browser.NewPageAsync().ConfigureAwait(false);
        }).As<Task<IPage>>();
    }

}
