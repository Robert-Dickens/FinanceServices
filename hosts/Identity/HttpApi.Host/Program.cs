using ByteLabs.FinanceServices;
using ByteLabs.FinanceServices.Services.Administration.Domain.Context;
using ByteLabs.FinanceServices.Services.Identity.Domain.Context;
using ByteLabs.FinanceServices.Services.Saas.Domain.Context;
using Serilog;

namespace FinanceServices.Services.IdentityService;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.InitializeHost();
            builder.InitializeSerilog();

            builder.AddNpgsqlDbContext<IdentityServiceDbContext>(GlobalConstants.Databases.IdentityServiceConnectionStringName, options =>
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

            await builder.AddApplicationAsync<IdentityServiceHttpApiHostModule>(options => options.ApplicationName = GlobalConstants.Services.IdentityServiceName);


            WebApplication app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, $"{GlobalConstants.Services.IdentityServiceName} terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
