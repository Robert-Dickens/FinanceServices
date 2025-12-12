using ByteLabs.Foundations.EntityFrameworkCore.Design;
using ByteLabs.Foundations.EntityFrameworkCore.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.Administration.Domain.Context.PostgreSql;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands)
 *
 * It is also used in the DbMigrator application.
 * */
public class AdministrationServiceDbContextFactory : PlatformDesignTimeDbContextBase<EntityFrameworkCorePostgreSqlModule, AdministrationServiceDbContext>
{
    protected override IConfigurationRoot BuildConfiguration()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddEnvironmentVariables()
            .AddInMemoryCollection(new Dictionary<string, string> { { $"ConnectionStrings:{AdministrationServiceDbProperties.ConnectionStringName}", "Server=localhost;Database=AdministrationService;Integrated Security=true;" }, { "HostingOptions:EnablePendingMigrations", "false" } });
        return configurationBuilder.Build();
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AdministrationServiceDbContext>(options =>
        {
            options.UseNpgsql(BuildConfiguration().GetConnectionString(AdministrationServiceDbProperties.ConnectionStringName), b => b.MigrationsAssembly(typeof(AdministrationPostgreSqlDomainContextModule).Assembly.FullName));
        });
    }
}