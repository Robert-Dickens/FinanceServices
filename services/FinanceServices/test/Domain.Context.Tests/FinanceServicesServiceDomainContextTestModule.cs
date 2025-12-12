using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore.Sqlite;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.Uow;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Testing;

[DependsOn(
    typeof(FinanceServicesServiceTestBaseModule),
    typeof(FinanceServicesServiceDomainContextModule),
    typeof(EntityFrameworkCoreSqliteModule)
)]
public class FinanceServicesServiceDomainContextTestModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAlwaysDisableUnitOfWorkTransaction();

        var sqliteConnection = CreateDatabaseAndGetConnection();

        Configure<PlatformDbContextOptions>(options =>
        {
            options.Configure<FinanceServicesServiceDbContext>(c =>
            {
                c.DbContextOptions.UseSqlite(sqliteConnection);
            });
        });
    }

    private static SqliteConnection CreateDatabaseAndGetConnection()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        AccessorExtensions.GetService<IRelationalDatabaseCreator>(new FinanceServicesServiceDbContext(
            new DbContextOptionsBuilder<FinanceServicesServiceDbContext>().UseSqlite(connection).Options
        )).CreateTables();

        return connection;
    }
}
