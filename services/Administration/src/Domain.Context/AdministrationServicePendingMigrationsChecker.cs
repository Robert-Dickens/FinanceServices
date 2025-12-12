using ByteLabs.FinanceServices.Services.Administration.Domain.Context;
using ByteLabs.Foundations.DataFactory.Data.DbMigrator;
using ByteLabs.Foundations.DistributedLocking;
using ByteLabs.Foundations.EventBus.Distributed;
using ByteLabs.Foundations.MultiTenancy;
using ByteLabs.Foundations.Uow;
using Microsoft.Extensions.Logging;

namespace ByteLabs.FinanceServices.Services.Administration.Domain;

public class AdministrationServicePendingMigrationsChecker : PendingEfCoreMigrationsChecker<AdministrationServiceDbContext>
{
    public AdministrationServicePendingMigrationsChecker(
        ILoggerFactory loggerFactory,
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        ICurrentTenant currentTenant,
        IDistributedEventBus distributedEventBus,
        IDistributedLock abpDistributedLock) : base(
        loggerFactory,
        unitOfWorkManager,
        serviceProvider,
        currentTenant,
        distributedEventBus,
        abpDistributedLock,
        AdministrationServiceDbProperties.ConnectionStringName)
    {
    }
}
