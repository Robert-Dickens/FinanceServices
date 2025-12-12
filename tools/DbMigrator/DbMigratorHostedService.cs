using ByteLabs.Foundations;
using ByteLabs.Foundations.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DbMigrator;

public class DbMigratorHostedService : IHostedService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IConfiguration _configuration;

    public DbMigratorHostedService(
        IHostApplicationLifetime hostApplicationLifetime,
        IConfiguration configuration)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var application = ApplicationPlatformFactory.Create<FinanceServicesDbMigratorModule>(options =>
        {
            options.Services.ReplaceConfiguration(_configuration);
            options.UseAutofac();
            options.Services.AddOpenTelemetry();
            options.Services.AddLogging(c => c.AddSerilog(new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "DbMigrator")
            .WriteTo.Async(c => c.Console())
            .WriteTo.Async(c => c.OpenTelemetry(options =>
            {
                options.Endpoint = _configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://localhost:4317";
                options.ResourceAttributes["service.name"] = "DbMigrator";
            })).CreateLogger()));
            options.AddDataMigrationEnvironment();
        }))
        {
            application.Initialize();

            await application
                .ServiceProvider
                .GetRequiredService<FinanceServicesDbMigrationService>()
                .MigrateAsync(cancellationToken);

            application.Shutdown();

            _hostApplicationLifetime.StopApplication();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
