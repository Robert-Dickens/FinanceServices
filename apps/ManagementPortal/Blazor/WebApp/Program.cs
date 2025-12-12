using ByteLabs.FinanceServices;
using Serilog;

namespace FinanceServices.ManagementPortal.Blazor;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.InitializeHost();
            builder.InitializeSerilog();

            builder.AddRedisClient(GlobalConstants.Services.RedisConnectionStringName);

            await builder.AddApplicationAsync<FinanceServicesBlazorWebAppModule>(options => options.ApplicationName = GlobalConstants.Clients.BlazorManagementPortalServiceName);
            WebApplication app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, $"{GlobalConstants.Clients.BlazorManagementPortalServiceName} terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
