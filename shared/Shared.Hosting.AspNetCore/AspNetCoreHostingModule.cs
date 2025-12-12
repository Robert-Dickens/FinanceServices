using ByteLabs.Aps.Hosting.AspNetCore;
using ByteLabs.Foundations.AspNetCore.MultiTenancy;
using ByteLabs.Foundations.Auditing;
using ByteLabs.Foundations.Caching;
using ByteLabs.Foundations.Modularity;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Hosting.AspNetCore;

[DependsOn(
    typeof(SharedHostingModule),
    typeof(AspNetCoreMultiTenancyModule),
    typeof(PlatformHostingAspNetCoreModule)
)]
public class AspNetCoreHostingModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        PreConfigure<ApplicationInsightsServiceOptions>(options =>
        {
            var configuration = context.Services.GetConfiguration();
            string? azureInsigtsInstrumentationKey = configuration.GetConnectionString("ApplicationInsights");
            var insightsSection = configuration.GetSection("ApplicationInsights");
            if (insightsSection.Exists())
            {
                insightsSection.Bind(options);
            }

            if (string.IsNullOrEmpty(options.ConnectionString) && !string.IsNullOrEmpty(azureInsigtsInstrumentationKey))
            {
                options.ConnectionString = azureInsigtsInstrumentationKey;
            }

            options.EnableDebugLogger = true;
            options.EnableAdaptiveSampling = true;
            options.EnableDependencyTrackingTelemetryModule = true;
            options.EnablePerformanceCounterCollectionModule = true;
            options.EnableAppServicesHeartbeatTelemetryModule = true;
            options.RequestCollectionOptions.TrackExceptions = true;
            options.EnableHeartbeat = true;
            options.EnableRequestTrackingTelemetryModule = true;
            options.EnableDiagnosticsTelemetryModule = true;
            options.EnableActiveTelemetryConfigurationSetup = true;

        });

    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {

        Configure<AuditingOptions>(options =>
        {
            options.ApplicationName = context.Services.GetApplicationRoleName();
        });


        Configure<AspNetCoreMultiTenancyOptions>(options =>
        {
            options.TenantKey = "tenantId";
        });

        var insightsPreConfigures = context.Services.GetPreConfigureActions<ApplicationInsightsServiceOptions>();

        context.Services.AddApplicationInsightsTelemetry(options =>
        {
            insightsPreConfigures?.Configure(options);
        });

        // track full SQL queries in telemetry
        context.Services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) => module.EnableSqlCommandTextInstrumentation = true);

    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        Configure<DistributedCacheOptions>(options =>
        {
            options.KeyPrefix = $"{context.Services.GetApplicationRoleName()}:";
        });

    }
}
