using ByteLabs.Aps.Hosting.Gateways.Yarp;
using ByteLabs.FinanceServices;
using ByteLabs.FinanceServices.Hosting.Gateways;
using ByteLabs.Foundations;
using ByteLabs.Foundations.AspNetCore;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.Swashbuckle;
using ByteLabs.Foundations.Threading;
using Microsoft.AspNetCore.Rewrite;

namespace FinanceServices.WebGateway.Yarp;

[DependsOn(
    typeof(AspNetCoreYarpGatewaysModule)
)]
public class FinanceServicesWebGatewayModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        SwaggerConfigurationHelper.ConfigureWithAuth(
        context: context,
        scopes: new
            Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
            {
               {GlobalConstants.Services.AccountServiceName, $"{GlobalConstants.Services.AccountServiceName} API"},
               {GlobalConstants.Services.IdentityServiceName, $"{GlobalConstants.Services.IdentityServiceName} API"},
               {GlobalConstants.Services.AdministrationServiceName, $"{GlobalConstants.Services.AdministrationServiceName} API"},
               {GlobalConstants.Services.SaasServiceName, $"{GlobalConstants.Services.SaasServiceName} API"},
               {GlobalConstants.Services.FinanceServicesServiceName, $"{GlobalConstants.Services.FinanceServicesServiceName} API"}
            },
            apiTitle: $"{context.Services.GetApplicationName()} API");

        context.Services.AddMemoryCache();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        IApplicationBuilder app = context.GetApplicationBuilder();
        IWebHostEnvironment env = context.GetEnvironment();
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCorrelationId();
        app.UseCors();
        app.UseAbpRequestLocalization();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.UseSwaggerUIWithYarp(context);
        app.UseAbpSerilogEnrichers();

        app.UseRewriter(new RewriteOptions()
            // Regex for "", "/" and "" (whitespace)
            .AddRedirect("^(|\\|\\s+)$", "/swagger"));

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapReverseProxyWithLocalization();
        });

    }
}
