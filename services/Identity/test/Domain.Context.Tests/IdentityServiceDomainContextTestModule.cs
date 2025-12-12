using ByteLabs.FinanceServices.Services.Identity.Domain;
using ByteLabs.FinanceServices.Services.Identity.Domain.Context;
using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore.Sqlite;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.Uow;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace ByteLabs.FinanceServices.Services.Identity.Testing;

[DependsOn(
    typeof(IdentityServiceTestBaseModule),
    typeof(IdentityServiceDomainContextModule),
    typeof(EntityFrameworkCoreSqliteModule)
)]
public class IdentityServiceDomainContextTestModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAlwaysDisableUnitOfWorkTransaction();
        
        var sqliteConnection = CreateDatabaseAndGetConnection();

        Configure<PlatformDbContextOptions>(options =>
        {
            options.Configure<IdentityServiceDbContext>(c =>
            {
                c.DbContextOptions.UseSqlite(sqliteConnection);
            });
        });
    }

    private static SqliteConnection CreateDatabaseAndGetConnection()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        AccessorExtensions.GetService<IRelationalDatabaseCreator>(new IdentityServiceDbContext(
            new DbContextOptionsBuilder<IdentityServiceDbContext>().UseSqlite(connection).Options
        )).CreateTables();

        return connection;
    }
}
