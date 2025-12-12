using ByteLabs.FinanceServices.Services.Identity.Domain.Context;
using ByteLabs.Foundations.DataFactory.Data.DbMigrator;
using ByteLabs.Foundations.DistributedLocking;
using ByteLabs.Foundations.EventBus.Distributed;
using ByteLabs.Foundations.MultiTenancy;
using ByteLabs.Foundations.Uow;
using Microsoft.Extensions.Logging;

namespace ByteLabs.FinanceServices.Services.Identity.Domain;

public class IdentityServicePendingMigrationsChecker : PendingEfCoreMigrationsChecker<IdentityServiceDbContext>
{
    public IdentityServiceDataSeeder DataSeeder { get; }

    public IdentityServicePendingMigrationsChecker(
        ILoggerFactory loggerFactory,
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        ICurrentTenant currentTenant,
        IDistributedEventBus distributedEventBus,
        IDistributedLock abpDistributedLock,
         IdentityServiceDataSeeder dataSeeder) : base(
        loggerFactory,
        unitOfWorkManager,
        serviceProvider,
        currentTenant,
        distributedEventBus,
        abpDistributedLock,
        IdentityServiceDbProperties.ConnectionStringName)
    {
        DataSeeder = dataSeeder;
    }

    public override async Task CheckAndApplyDatabaseMigrationsAsync()
    {
        await base.CheckAndApplyDatabaseMigrationsAsync();
        await TryAsync(async () => await DataSeeder.SeedAsync());
    }

}
