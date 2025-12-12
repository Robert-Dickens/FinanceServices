using ByteLabs.Foundations.AspNetCore.UI.Bundling;

namespace FinanceServices.ManagementPortal.Blazor.WebAssembly;

public class FinanceServicesBlazorWebAssemblyBundleContributor : IBundleContributor
{
    public Task ConfigureBundleAsync(BundleConfigurationContext context)
    {
        return Task.CompletedTask;
    }

    public Task ConfigureDynamicResourcesAsync(BundleConfigurationContext context)
    {
        return Task.CompletedTask;
    }

    public Task PostConfigureBundleAsync(BundleConfigurationContext context)
    {
        return Task.CompletedTask;
    }

    public Task PreConfigureBundleAsync(BundleConfigurationContext context)
    {
        return Task.CompletedTask;
    }
}
