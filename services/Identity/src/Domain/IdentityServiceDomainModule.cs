using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.AuditLogging.Domain;
using ByteLabs.PlatformServices.Identity.Domain;
using ByteLabs.PlatformServices.Permissions.Domain;
using ByteLabs.PlatformServices.Security.IdentityServer;
using ByteLabs.PlatformServices.Security.IdentityServer.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.Identity.Domain;

[DependsOn(
    typeof(OpenApiSecuritySharedModule),
    typeof(IdentityDomainModule),
    typeof(AuditLoggingDomainModule),
    typeof(PermissionManagementDomainModule),
    typeof(IdentityServerDomainModule),
    typeof(IdentityServiceDomainAbstractionsModule))]
public class IdentityServiceDomainModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IdentityServiceDataSeeder>();
    }
}
