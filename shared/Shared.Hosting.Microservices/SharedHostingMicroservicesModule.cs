using ByteLabs.Aps.Hosting.Microservices;
using ByteLabs.FinanceServices.Hosting.AspNetCore;
using ByteLabs.FinanceServices.Hosting.Distributed;
using ByteLabs.FinanceServices.Services.Administration.Domain.Context.PostgreSql;
using ByteLabs.FinanceServices.Services.Saas.Domain.Context.PostgreSql;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.Security.OpenTrust.Options;
using ByteLabs.Foundations.Swashbuckle;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Hosting.Microservices;

[DependsOn(typeof(AspNetCoreHostingModule), typeof(DistributedServicesHostingModule), typeof(PlatformHostingMicroservicesModule))]
[DependsOn(typeof(SaasPostgreSqlDomainContextModule), typeof(AdministrationPostgreSqlDomainContextModule))]
public class SharedHostingMicroservicesModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        PreConfigure<SecuritySection>(options =>
        {
            options.ConfigureAuthorization(provider =>
            {
                provider.AuthorizationType = ByteLabs.Foundations.Security.OpenTrust.AuthorizationType.JwtBearer;
                provider.Authority = configuration["AuthServer:Authority"];
                provider.IsDefault = true;
                provider.Audience = context.Services.GetApplicationName();
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureSwaggerServices(context);
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context)
    {
        var appName = context.Services.GetApplicationName();
        SwaggerConfigurationHelper.ConfigureWithAuth(
                context: context,
                scopes: new
                    Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
                    {
                                {appName, $"{appName} API"}
                    },
                apiTitle: $"{appName} API");
    }

}
