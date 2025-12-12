using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore.PostgreSql;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Domain.Context.PostgreSql;

[DependsOn(
    typeof(EntityFrameworkCorePostgreSqlModule),
    typeof(FinanceServicesServiceDomainContextModule)
)]
public class FinanceServicesServicePostgreSqlDomainContextModule : PlatformModule
{

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PlatformDbContextOptions>(options =>
        {
            options.Configure<FinanceServicesServiceDbContext>(ctx =>
            {
                ctx.UseNpgsql(d => d.MigrationsAssembly(typeof(FinanceServicesServicePostgreSqlDomainContextModule).Assembly.FullName));
            });
        });
    }
}
