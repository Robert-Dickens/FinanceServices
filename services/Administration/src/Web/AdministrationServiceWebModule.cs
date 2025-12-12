using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.VirtualFileSystem;
using ByteLabs.Platform.Foundations.Web;
using ByteLabs.PlatformServices.Account.Admin.Web;

namespace ByteLabs.FinanceServices.Services.Administration.Web;

[DependsOn(
    typeof(AccountAdminWebModule),
    typeof(AdministrationServiceApplicationAbstractionsModule),
    typeof(AdministrationServiceHttpApiModule),
    typeof(PlatformServicesWebModule)
    )]
public class AdministrationServiceWebModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<VirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AdministrationServiceWebModule>();
        });

        context.Services.AddAutoMapperObjectMapper<AdministrationServiceWebModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AdministrationServiceWebModule>(validate: true);
        });
    }
}
