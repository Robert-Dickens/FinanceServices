using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore.SqlServer;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Domain.Context.SqlServer;

[DependsOn(typeof(EntityFrameworkCoreSqlServerModule), typeof(FinanceServicesServiceDomainContextModule))]
public class FinanceServicesServiceSqlServerDomainContextModule : PlatformModule
{

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PlatformDbContextOptions>(options =>
        {
            options.Configure<FinanceServicesServiceDbContext>(ctx =>
            {
                ctx.UseSqlServer(d => d.MigrationsAssembly(typeof(FinanceServicesServiceSqlServerDomainContextModule).Assembly.FullName));
            });
        });
    }
}
