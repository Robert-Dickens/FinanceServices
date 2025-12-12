using ByteLabs.FinanceServices;
using Serilog;

namespace FinanceServices.ManagementPortal.Web;

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
            //builder.AddRabbitMQClient(GlobalConstants.Services.RabbitMqStringName);
            //builder.AddSqlServerClient(GlobalConstants.Databases.AdministrationServiceConnectionStringName);

            await builder.AddApplicationAsync<FinanceServicesWebModule>(options => options.ApplicationName = GlobalConstants.Clients.ManagementPortalServiceName);
            WebApplication app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, $"{GlobalConstants.Clients.ManagementPortalServiceName} terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
