using ByteLabs.FinanceServices.Services.Saas.Domain.Context;
using ByteLabs.Foundations.DataFactory.Data.DbMigrator;
using ByteLabs.Foundations.DistributedLocking;
using ByteLabs.Foundations.EventBus.Distributed;
using ByteLabs.Foundations.MultiTenancy;
using ByteLabs.Foundations.Uow;
using Microsoft.Extensions.Logging;

namespace ByteLabs.FinanceServices.Services.Saas.Domain;

public class SaasServicePendingMigrationsChecker : PendingEfCoreMigrationsChecker<SaasServiceDbContext>
{
    public SaasServicePendingMigrationsChecker(
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
        SaasServiceDbProperties.ConnectionStringName)
    {
    }
}
