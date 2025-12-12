using ByteLabs.Foundations.Hosting;
using ByteLabs.Foundations.Messaging.Email;
using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ByteLabs.FinanceServices.Hosting;

[DependsOn(
    typeof(PlatformHostingModule)
)]
public class SharedHostingModule : PlatformModule
{

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
    }
}
