using ByteLabs.FinanceServices.Services.Saas.Domain;
using ByteLabs.Foundations;
using ByteLabs.Foundations.Modularity;
using FinanceServices.Shared.Hosting;
using Prometheus;
using ByteLabs.Foundations.AspNetCore;


namespace FinanceServices.Services.Saas;

[DependsOn(
    typeof(SharedHostingMicroservicesModule),
    typeof(SaasServiceDomainContextModule),
    typeof(SaasServiceApplicationModule),
    typeof(SaasServiceHttpApiModule)
)]
[DependsOn(typeof(SaasPostgreSqlDomainContextModule))]
public class SaasServiceHttpApiHostModule : PlatformModule
{

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        var configuration = context.GetConfiguration();
        var openApiContext = context.GetOpenApiSwaggerContext();

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
