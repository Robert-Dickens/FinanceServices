using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.VirtualFileSystem;
using ByteLabs.PlatformServices.Identity.Web;
using ByteLabs.PlatformServices.Security.IdentityServer.Web;

namespace ByteLabs.FinanceServices.Services.Identity.Web;

[DependsOn(
    typeof(IdentityWebModule),
    typeof(IdentityServerManagementWebModule),
    typeof(IdentityServiceApplicationAbstractionsModule),
    typeof(IdentityServiceHttpApiModule)
)]
public class IdentityServiceWebModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<VirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<IdentityServiceWebModule>();
        });

        context.Services.AddAutoMapperObjectMapper<IdentityServiceWebModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<IdentityServiceWebModule>(validate: true);
        });
    }
}
