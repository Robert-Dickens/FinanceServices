using ByteLabs.Foundations.DistributedSystems.CAP;
using ByteLabs.Foundations.DistributedSystems.CAP.PostgreSql;
using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore.PostgreSql;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Services.Administration.Domain.Context.PostgreSql;

[DependsOn(
    typeof(EntityFrameworkCorePostgreSqlModule), 
    typeof(PlatformEventBusCapPostgreSqlStorageProviderModule), 
    typeof(AdministrationServiceDomainContextModule)
    )]
public class AdministrationPostgreSqlDomainContextModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        PreConfigure<PlatformCAPEventBusOptions>(options =>
        {
            options.StorageProvider = StorageProviderType.PostgreSql;
            options.ConnectionStringName = AdministrationServiceDbProperties.ConnectionStringName;
        });
    }


    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PlatformDbContextOptions>(options =>
        {
            options.Configure<AdministrationServiceDbContext>(ctx =>
            {
                ctx.UseNpgsql(d => d.MigrationsAssembly(typeof(AdministrationPostgreSqlDomainContextModule).Assembly.FullName));
            });
        });
    }
}
