using ByteLabs.Foundations.EntityFrameworkCore.Design;
using ByteLabs.Foundations.EntityFrameworkCore.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.Identity.Domain.Context.PostgreSql;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands)
 *
 * It is also used in the DbMigrator application.
 * */
public class IdentityServiceDbContextFactory : PlatformDesignTimeDbContextBase<EntityFrameworkCorePostgreSqlModule, IdentityServiceDbContext>
{
    protected override IConfigurationRoot BuildConfiguration()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddEnvironmentVariables()
            .AddInMemoryCollection(new Dictionary<string, string> { { $"ConnectionStrings:{IdentityServiceDbProperties.ConnectionStringName}", "Server=localhost;Database=IdentityService;Integrated Security=true;" }, { "HostingOptions:EnablePendingMigrations", "false" } });
        return configurationBuilder.Build();
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<IdentityServiceDbContext>(options =>
        {
            options.UseNpgsql(BuildConfiguration().GetConnectionString(IdentityServiceDbProperties.ConnectionStringName), b => b.MigrationsAssembly(typeof(IdentityPostgreSqlDomainContextModule).Assembly.FullName));
        });
    }
}