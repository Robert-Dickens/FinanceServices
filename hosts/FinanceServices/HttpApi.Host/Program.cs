using ByteLabs.FinanceServices;
using ByteLabs.FinanceServices.Services.Administration.Domain.Context;
using ByteLabs.FinanceServices.Services.FinanceServices.Domain.Context;
using ByteLabs.FinanceServices.Services.Saas.Domain.Context;
using Serilog;

namespace FinanceServices.Services.FinanceServicesService;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.InitializeHost();
            builder.InitializeSerilog();

            builder.AddNpgsqlDbContext<FinanceServicesServiceDbContext>(GlobalConstants.Databases.FinanceServicesConnectionStringName, options =>
            {
                options.DisableRetry = true;
            });


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


            await builder.AddApplicationAsync<FinanceServicesServiceHttpApiHostModule>(options => options.ApplicationName = GlobalConstants.Services.FinanceServicesServiceName);
            WebApplication app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, $"{GlobalConstants.Services.FinanceServicesServiceName} terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
