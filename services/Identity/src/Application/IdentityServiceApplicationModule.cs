using ByteLabs.FinanceServices.Services.Identity.Domain;
using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.Account;
using ByteLabs.PlatformServices.Identity;
using ByteLabs.PlatformServices.Security.IdentityServer;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.Identity;

[DependsOn(
    typeof(IdentityApplicationModule),
    typeof(IdentityServerApplicationModule),
    typeof(IdentityServiceDomainModule),
    typeof(AccountAdminApplicationModule),
    typeof(AccountPublicApplicationModule),
    typeof(IdentityServiceApplicationAbstractionsModule)
)]
public class IdentityServiceApplicationModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<IdentityServiceApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<IdentityServiceApplicationModule>(validate: true);
        });
    }
}
