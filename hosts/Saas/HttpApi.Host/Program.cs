using ByteLabs.FinanceServices.Services.Saas.Domain.Context;
using FinanceServices.Shared;
using Serilog;

namespace FinanceServices.Services.Saas.Host;

public class Program
{
    public async static Task<int> Main(string[] args)
    {

        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.InitializeHost();
            builder.InitializeSerilog();

            builder.AddNpgsqlDbContext<AdministrationServiceDbContext>(GlobalConstants.Databases.AdministrationServiceConnectionStringName, options =>
            {
                options.DisableRetry = true;
            });

            builder.AddNpgsqlDbContext<SaasServiceDbContext>(GlobalConstants.Databases.SaasServiceConnectionStringName, options =>
            {
                options.DisableRetry = true;
            });



            builder.AddRedisClient(GlobalConstants.Services.RedisConnectionStringName);
            builder.AddRabbitMQClient(GlobalConstants.Services.RabbitMqStringName);


            await builder.AddApplicationAsync<SaasServiceHttpApiHostModule>(options => options.ApplicationName = GlobalConstants.Services.SaasServiceName);
            WebApplication app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, $"{GlobalConstants.Services.SaasServiceName} terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
