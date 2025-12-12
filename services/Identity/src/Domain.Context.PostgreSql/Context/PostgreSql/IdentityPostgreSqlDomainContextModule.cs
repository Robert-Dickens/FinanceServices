using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore.PostgreSql;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.Identity.Domain.Context.PostgreSql;

[DependsOn(
    typeof(EntityFrameworkCorePostgreSqlModule),
    typeof(IdentityServiceDomainContextModule)
)]
public class IdentityPostgreSqlDomainContextModule : PlatformModule
{

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PlatformDbContextOptions>(options =>
        {
            options.Configure<IdentityServiceDbContext>(ctx =>
            {
                ctx.UseNpgsql(d => d.MigrationsAssembly(typeof(IdentityPostgreSqlDomainContextModule).Assembly.FullName));
            });
        });
    }
}
