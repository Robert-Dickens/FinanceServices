using ByteLabs.Aps.Hosting.Distributed;
using ByteLabs.FinanceServices.Hosting.AspNetCore;
using ByteLabs.Foundations.BackgroundServices.BackgroundJobs.RabbitMQ;
using ByteLabs.Foundations.DistributedSystems.CAP;
using ByteLabs.Foundations.DistributedSystems.CAP.RabbitMQ;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.RabbitMQ;

namespace ByteLabs.FinanceServices.Hosting.Distributed;

[DependsOn(
    typeof(PlatformEventBusCapRabbitMqModule),
    typeof(AspNetCoreHostingModule),
    typeof(PlatformHostingDistributedServicesModule),
    typeof(BackgroundJobsRabbitMqModule)
  )]
public class DistributedServicesHostingModule : PlatformModule
{

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<CapRabbitMqOptions>(options =>
        {
            options.RabbitMqProfileName = RabbitMqConnections.DefaultConnectionName;
        });

        PreConfigure<PlatformCAPEventBusOptions>(options =>
        {
            options.EventBusProvider = EventBusType.RabbitMQ;
        });
    }
}
