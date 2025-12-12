using ByteLabs.FinanceServices.Hosting.Microservices;
using ByteLabs.FinanceServices.Services.FinanceServices;
using ByteLabs.FinanceServices.Services.FinanceServices.Domain.Context.PostgreSql;
using ByteLabs.Foundations;
using ByteLabs.Foundations.AspNetCore;
using ByteLabs.Foundations.Modularity;
using Prometheus;


namespace FinanceServices.Services.FinanceServicesService;

[DependsOn(typeof(SharedHostingMicroservicesModule))]
[DependsOn(
    typeof(FinanceServicesServiceApplicationModule),
    typeof(FinanceServicesServiceHttpApiModule)
)]
[DependsOn(typeof(FinanceServicesServicePostgreSqlDomainContextModule))]
public class FinanceServicesServiceHttpApiHostModule : PlatformModule
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
        app.UseAuthentication();
        app.UseAbpClaimsMap();
        app.UseMultiTenancy();
        app.UseAuthorization();
        app.UseAbpSerilogEnrichers();
        app.UseAuditing();
        app.UseUnitOfWork();
        app.UseConfiguredEndpoints(endpoints => endpoints.MapMetrics());
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
