using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DbMigrator;

class Program
{
    async static Task Main(string[] args)
    {
        await CreateHostBuilder(args).RunConsoleAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .AddAppSettingsSecretsJson()
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.OpenIdDefaults.json", optional: true, reloadOnChange: true);
                config.AddEnvironmentVariables();
                config.AddCommandLine(args);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.EnableServiceDiscovery();
                services.AddHostedService<DbMigratorHostedService>();
            })
            .ConfigureLogging((context, logging) =>
            {
                var otelEndpoint = context.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://localhost:4317";
                logging.AddSerilog(new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "DbMigrator")
                .WriteTo.Async(c => c.Console())
                .WriteTo.Async(c => c.OpenTelemetry(options =>
                {
                    options.Endpoint = otelEndpoint;
                    options.ResourceAttributes["service.name"] = "DbMigrator";
                }))
                .CreateLogger());
            });

}
