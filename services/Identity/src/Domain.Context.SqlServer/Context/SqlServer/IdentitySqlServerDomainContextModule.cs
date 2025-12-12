using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore.SqlServer;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.Identity.Domain.Context.SqlServer;

[DependsOn(typeof(EntityFrameworkCoreSqlServerModule), typeof(IdentityServiceDomainContextModule))]
public class IdentitySqlServerDomainContextModule : PlatformModule
{

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PlatformDbContextOptions>(options =>
        {
            options.Configure<IdentityServiceDbContext>(ctx =>
            {
                ctx.UseSqlServer(d => d.MigrationsAssembly(typeof(IdentitySqlServerDomainContextModule).Assembly.FullName));
            });
        });
    }
}
