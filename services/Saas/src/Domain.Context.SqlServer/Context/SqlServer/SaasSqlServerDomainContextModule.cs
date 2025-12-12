using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore.SqlServer;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.Saas.Domain.Context.SqlServer;

[DependsOn(typeof(EntityFrameworkCoreSqlServerModule), typeof(SaasServiceDomainContextModule))]
public class SaasSqlServerDomainContextModule : PlatformModule
{

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PlatformDbContextOptions>(options =>
        {
            options.Configure<SaasServiceDbContext>(ctx =>
            {
                ctx.UseSqlServer(d => d.MigrationsAssembly(typeof(SaasSqlServerDomainContextModule).Assembly.FullName));
            });
        });
    }
}
