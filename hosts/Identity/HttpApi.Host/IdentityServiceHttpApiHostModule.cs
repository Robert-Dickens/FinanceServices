using ByteLabs.FinanceServices.Services.Identity;
using ByteLabs.FinanceServices.Services.Identity.Domain.Context.PostgreSql;
using ByteLabs.Foundations;
using ByteLabs.Foundations.Modularity;
using FinanceServices.Shared.Hosting;
using Prometheus;
using ByteLabs.Foundations.AspNetCore;
using ByteLabs.Foundations.BlobStoring;
using ByteLabs.Foundations.BlobStoring.FileSystem;




namespace FinanceServices.Services.IdentityService;

[DependsOn(
    typeof(SharedHostingMicroservicesModule),
    typeof(IdentityServiceApplicationModule),
    typeof(IdentityServiceHttpApiModule),
    typeof(BlobStoringFileSystemModule)
)]
[DependsOn(typeof(IdentityPostgreSqlDomainContextModule))]
public class IdentityServiceHttpApiHostModule : PlatformModule
{

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<BlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                container.UseFileSystem(fileSystem =>
                {
                    fileSystem.BasePath = context.Services.GetHostingEnvironment().ContentRootPath;
                });
            });
        });
    }


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
