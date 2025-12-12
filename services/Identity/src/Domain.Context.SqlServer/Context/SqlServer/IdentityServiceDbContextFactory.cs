using ByteLabs.Foundations.EntityFrameworkCore.Design;
using ByteLabs.Foundations.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.Identity.Domain.Context.SqlServer;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands)
 *
 * It is also used in the DbMigrator application.
 * */
public class IdentityServiceDbContextFactory : PlatformDesignTimeDbContextBase<EntityFrameworkCoreSqlServerModule, IdentityServiceDbContext>
{
    protected override IConfigurationRoot BuildConfiguration()
    {
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
            options.UseSqlServer(BuildConfiguration().GetConnectionString(IdentityServiceDbProperties.ConnectionStringName), b => b.MigrationsAssembly(typeof(IdentitySqlServerDomainContextModule).Assembly.FullName));
        });
    }
}