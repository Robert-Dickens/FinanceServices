using ByteLabs.FinanceServices.Services.Saas.Domain;
using ByteLabs.FinanceServices.Services.Saas.Domain.Context;
using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore.Sqlite;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.Uow;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace ByteLabs.FinanceServices.Services.Saas.Testing;

[DependsOn(
    typeof(SaasServiceTestBaseModule),
    typeof(SaasServiceDomainContextModule),
    typeof(EntityFrameworkCoreSqliteModule)
)]
public class SaasServiceDomainContextTestModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAlwaysDisableUnitOfWorkTransaction();

        var sqliteConnection = CreateDatabaseAndGetConnection();

        Configure<PlatformDbContextOptions>(options =>
        {
            options.Configure<SaasServiceDbContext>(c =>
            {
                c.DbContextOptions.UseSqlite(sqliteConnection);
            });
        });
    }

    private static SqliteConnection CreateDatabaseAndGetConnection()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        new SaasServiceDbContext(
            new DbContextOptionsBuilder<SaasServiceDbContext>().UseSqlite(connection).Options
        ).GetService<IRelationalDatabaseCreator>().CreateTables();

        return connection;
    }
}
