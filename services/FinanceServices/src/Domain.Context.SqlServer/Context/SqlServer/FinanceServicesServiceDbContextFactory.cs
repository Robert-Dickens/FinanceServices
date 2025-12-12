using ByteLabs.Foundations.EntityFrameworkCore.Design;
using ByteLabs.Foundations.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Domain.Context.SqlServer;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands)
 *
 * It is also used in the DbMigrator application.
 * */
public class FinanceServicesServiceDbContextFactory : PlatformDesignTimeDbContextBase<EntityFrameworkCoreSqlServerModule, FinanceServicesServiceDbContext>
{
    protected override IConfigurationRoot BuildConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddEnvironmentVariables()
            .AddInMemoryCollection(new Dictionary<string, string> { { $"ConnectionStrings:{FinanceServicesServiceDbProperties.ConnectionStringName}", "Server=localhost;Database=FinanceServicesService;Integrated Security=true;" }, { "HostingOptions:EnablePendingMigrations", "false" } });
        return configurationBuilder.Build();
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<FinanceServicesServiceDbContext>(options =>
        {
            options.UseSqlServer(BuildConfiguration().GetConnectionString(FinanceServicesServiceDbProperties.ConnectionStringName), b => b.MigrationsAssembly(typeof(FinanceServicesServiceSqlServerDomainContextModule).Assembly.FullName));
        });
    }
}