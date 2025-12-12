using ByteLabs.Aps.Hosting.Gateways;
using ByteLabs.FinanceServices;
using Serilog;

namespace FinanceServices.WebGateway.Yarp;

public class Program
{
    public async static Task<int> Main(string[] args)
    {

        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.InitializeHost();
            builder.Host.AddYarpJson();
            builder.InitializeSerilog();

            builder.AddRedisClient(GlobalConstants.Services.RedisConnectionStringName);

            await builder.AddApplicationAsync<FinanceServicesWebGatewayModule>(options => options.ApplicationName = GlobalConstants.Services.GatewayServiceName);
            WebApplication app = builder.Build();

            await app.InitializeApplicationAsync();

            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, $"{GlobalConstants.Services.GatewayServiceName} terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
