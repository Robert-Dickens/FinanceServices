using FinanceServices.Shared;
using Serilog;

namespace FinanceServices.PublicServer.Web;

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


            await builder.AddApplicationAsync<FinanceServicesPublicWebModule>(options => options.ApplicationName = GlobalConstants.Clients.PublicPortalServiceName);
            WebApplication app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, $"{GlobalConstants.Clients.PublicPortalServiceName} terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
