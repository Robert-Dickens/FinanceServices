using ByteLabs.FinanceServices.Recevables.Localization;
using ByteLabs.FinanceServices.Recevables.Permissions;
using ByteLabs.FinanceServices.Recevables.Web.Menus;
using ByteLabs.Foundations.AspNetCore.Mvc.Localization;
using ByteLabs.Foundations.AspNetCore.Mvc.UI.Theme.Shared;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.VirtualFileSystem;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ByteLabs.FinanceServices.Recevables.Web;

[DependsOn(
    typeof(RecevablesApplicationAbstractionsModule),
    typeof(AspNetCoreMvcUiThemeSharedModule),
    typeof(AbpAutoMapperModule)
    )]
public class RecevablesWebModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<MvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(RecevablesResource), typeof(RecevablesWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(RecevablesWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AspNetCoreNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new RecevablesMenuContributor());
        });

        Configure<VirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<RecevablesWebModule>();
        });

        context.Services.AddAutoMapperObjectMapper<RecevablesWebModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<RecevablesWebModule>(validate: true);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Products/Index", RecevablesPermissions.Products.Default);
        });
    }
}
