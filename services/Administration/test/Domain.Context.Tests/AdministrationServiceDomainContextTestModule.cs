using ByteLabs.FinanceServices.Services.Administration.Domain;
using ByteLabs.FinanceServices.Services.Administration.Domain.Context;
using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore.Sqlite;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.Uow;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace ByteLabs.FinanceServices.Services.Administration.Testing;

[DependsOn(
    typeof(AdministrationServiceTestBaseModule),
    typeof(AdministrationServiceDomainContextModule),
    typeof(EntityFrameworkCoreSqliteModule)
)]
public class AdministrationServiceDomainContextTestModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAlwaysDisableUnitOfWorkTransaction();

        var sqliteConnection = CreateDatabaseAndGetConnection();

        Configure<PlatformDbContextOptions>(options =>
        {
            options.Configure<AdministrationServiceDbContext>(c =>
            {
                c.DbContextOptions.UseSqlite(sqliteConnection);
            });
        });
    }

    private static SqliteConnection CreateDatabaseAndGetConnection()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        AccessorExtensions.GetService<IRelationalDatabaseCreator>(new AdministrationServiceDbContext(
            new DbContextOptionsBuilder<AdministrationServiceDbContext>().UseSqlite(connection).Options
        )).CreateTables();

        return connection;
    }
}
