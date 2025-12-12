using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.VirtualFileSystem;
using ByteLabs.PlatformServices.SaaS.Web;

namespace ByteLabs.FinanceServices.Services.Saas.Web;

[DependsOn(
    typeof(SaasServiceApplicationAbstractionsModule),
    typeof(SaasWebModule),
    typeof(SaasServiceHttpApiModule)
)]
public class SaasServiceWebModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<VirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<SaasServiceWebModule>();
        });
    }
}
