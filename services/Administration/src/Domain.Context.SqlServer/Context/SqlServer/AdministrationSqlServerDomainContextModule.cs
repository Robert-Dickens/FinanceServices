using ByteLabs.Foundations.DistributedSystems.CAP;
using ByteLabs.Foundations.DistributedSystems.CAP.SqlServer;
using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore.SqlServer;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.Administration.Domain.Context.SqlServer;

[DependsOn(
    typeof(EntityFrameworkCoreSqlServerModule), 
    typeof(PlatformEventBusCapMsSqlStorageProviderModule), 
    typeof(AdministrationServiceDomainContextModule)
    )]
public class AdministrationSqlServerDomainContextModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<PlatformCAPEventBusOptions>(options =>
        {
            options.StorageProvider = StorageProviderType.MsSql;
            options.ConnectionStringName = AdministrationServiceDbProperties.ConnectionStringName;
        });

    }


    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PlatformDbContextOptions>(options =>
        {
            options.Configure<AdministrationServiceDbContext>(ctx =>
            {
                ctx.UseSqlServer(d => d.MigrationsAssembly(typeof(AdministrationSqlServerDomainContextModule).Assembly.FullName));
            });
        });


    }


}
