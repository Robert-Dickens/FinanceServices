using ByteLabs.FinanceServices.Payables.Domain;
using ByteLabs.FinanceServices.Payables.Domain.Context;
using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore.Sqlite;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.Uow;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace ByteLabs.FinanceServices.Payables.Testing;

[DependsOn(
    typeof(PayablesTestBaseModule),
    typeof(PayablesDomainContextModule),
    typeof(EntityFrameworkCoreSqliteModule)
)]
public class PayablesDomainContextTestModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAlwaysDisableUnitOfWorkTransaction();

        var sqliteConnection = CreateDatabaseAndGetConnection();

        Configure((PlatformDbContextOptions options) =>
        {
            options.Configure<PayablesDbContext>(c =>
            {
                c.DbContextOptions.UseSqlite(sqliteConnection);
            });
        });
    }

    private static SqliteConnection CreateDatabaseAndGetConnection()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        new PayablesDbContext(
            new DbContextOptionsBuilder<PayablesDbContext>().UseSqlite(connection).Options
        ).GetService<IRelationalDatabaseCreator>().CreateTables();

        return connection;
    }
}
