using ByteLabs.Foundations.DistributedLocking;
using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.EventBus.Distributed;
using ByteLabs.Foundations.MultiTenancy;
using ByteLabs.Foundations.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ByteLabs.FinanceServices.Hosting.Microservices.DbMigrations.EfCore;

public abstract class PendingEfCoreMigrationsChecker<TDbContext> : PendingMigrationsCheckerBase where TDbContext : DbContext, IEfCoreDbContext
{
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IServiceProvider ServiceProvider { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IDistributedEventBus DistributedEventBus { get; }
    protected IDistributedLock DistributedLock { get; }
    protected ILogger<PendingEfCoreMigrationsChecker<TDbContext>> Logger { get; }
    protected string DatabaseName { get; }

    protected PendingEfCoreMigrationsChecker(
        ILoggerFactory loggerFactory,
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        ICurrentTenant currentTenant,
        IDistributedEventBus distributedEventBus,
        IDistributedLock abpDistributedLock,
        string databaseName) : base(loggerFactory)
    {
        UnitOfWorkManager = unitOfWorkManager;
        ServiceProvider = serviceProvider;
        CurrentTenant = currentTenant;
        DistributedEventBus = distributedEventBus;
        DistributedLock = abpDistributedLock;
        DatabaseName = databaseName;
        Logger = loggerFactory.CreateLogger<PendingEfCoreMigrationsChecker<TDbContext>>();
    }

    public virtual async Task CheckAndApplyDatabaseMigrationsAsync()
    {
        await TryAsync(LockAndApplyDatabaseMigrationsAsync);
    }

    protected virtual async Task LockAndApplyDatabaseMigrationsAsync()
    {
        await using (var handle = await DistributedLock.TryAcquireAsync("Migration_" + DatabaseName))
        {
            Logger.LogInformation("Lock is acquired for db migration and seeding on database named: {DatabaseName}...", DatabaseName);

            if (handle is null)
            {
                Logger.LogInformation("Handle is null because of the locking for : {DatabaseName}", DatabaseName);
                return;
            }

            using (CurrentTenant.Change(null))
            {
                // Create database tables if needed
                using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false))
                {
                    var dbContext = await ServiceProvider
                        .GetRequiredService<IDbContextProvider<TDbContext>>()
                        .GetDbContextAsync();

                    var pendingMigrations = await dbContext
                        .Database
                        .GetPendingMigrationsAsync();

                    if (pendingMigrations.Any())
                    {
                        await dbContext.Database.MigrateAsync();
                    }

                    await uow.CompleteAsync();
                }
            }

            Logger.LogInformation("Lock is released for db migration and seeding on database named: {DatabaseName}...", DatabaseName);
        }
    }
}
