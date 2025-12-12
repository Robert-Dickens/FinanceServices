using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore.PostgreSql;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.Saas.Domain.Context.PostgreSql;

[DependsOn(typeof(EntityFrameworkCorePostgreSqlModule), typeof(SaasServiceDomainContextModule))]
public class SaasPostgreSqlDomainContextModule : PlatformModule
{

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PlatformDbContextOptions>(options =>
        {
            options.Configure<SaasServiceDbContext>(ctx =>
            {
                ctx.UseNpgsql(d => d.MigrationsAssembly(typeof(SaasPostgreSqlDomainContextModule).Assembly.FullName));
            });
        });

    }



}
