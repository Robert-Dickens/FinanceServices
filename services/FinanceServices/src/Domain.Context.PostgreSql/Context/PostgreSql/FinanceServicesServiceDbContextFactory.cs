using ByteLabs.Foundations.EntityFrameworkCore.Design;
using ByteLabs.Foundations.EntityFrameworkCore.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Domain.Context.PostgreSql;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands)
 *
 * It is also used in the DbMigrator application.
 * */
public class FinanceServicesServiceDbContextFactory : PlatformDesignTimeDbContextBase<EntityFrameworkCorePostgreSqlModule, FinanceServicesServiceDbContext>
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
            options.UseNpgsql(BuildConfiguration().GetConnectionString(FinanceServicesServiceDbProperties.ConnectionStringName), b => b.MigrationsAssembly(typeof(FinanceServicesServicePostgreSqlDomainContextModule).Assembly.FullName));
        });
    }
}