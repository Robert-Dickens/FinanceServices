using ByteLabs.FinanceServices.Hosting.Microservices;
using ByteLabs.FinanceServices.Localization;
using ByteLabs.FinanceServices.Services.Administration;
using ByteLabs.FinanceServices.Services.Administration.Domain;
using ByteLabs.FinanceServices.Services.FinanceServices;
using ByteLabs.FinanceServices.Services.Identity;
using ByteLabs.FinanceServices.Services.Saas;
using ByteLabs.Foundations;
using ByteLabs.Foundations.AspNetCore;
using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.Account;
using Prometheus;

namespace FinanceServices.Services.AdministrationService;

[DependsOn(
    typeof(SharedLocalizationModule),
    typeof(AccountAdminApplicationAbstractionsModule),
    typeof(SharedHostingMicroservicesModule),
    typeof(FinanceServicesServiceApplicationAbstractionsModule),
    typeof(SaasServiceApplicationAbstractionsModule),
    typeof(IdentityServiceApplicationAbstractionsModule),
    typeof(AdministrationServiceApplicationModule),
    typeof(AdministrationServiceDomainContextModule),
    typeof(AdministrationServiceHttpApiModule)
)]
public class AdministrationServiceHttpApiHostModule : PlatformModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        IApplicationBuilder app = context.GetApplicationBuilder();
        IWebHostEnvironment env = context.GetEnvironment();
        IConfiguration configuration = context.GetConfiguration();
        (bool isEnabled, bool isUiEnabled) openApiContext = context.GetOpenApiSwaggerContext();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCorrelationId();
        app.UseAbpRequestLocalization();
        app.UseAbpSecurityHeaders();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseHttpMetrics();
        app.UseAuthentication();
        app.UseAbpClaimsMap();
        app.UseMultiTenancy();
        app.UseAuthorization();
        app.UseAbpSerilogEnrichers();
        app.UseAuditing();
        app.UseUnitOfWork();
        app.UseConfiguredEndpoints(endpoints =>
        {
            endpoints.MapMetrics();
        });
        if (openApiContext.isEnabled)
            app.UseSwagger();
        if (openApiContext.isUiEnabled)
            app.UseOpenApiSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{app.GetApplicationName()} API");
                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
            });
    }
}
