using ByteLabs.FinanceServices.Payables.Localization;
using ByteLabs.FinanceServices.Payables.Permissions;
using ByteLabs.FinanceServices.Payables.Web.Menus;
using ByteLabs.Foundations.AspNetCore.Mvc.Localization;
using ByteLabs.Foundations.AspNetCore.Mvc.UI.Theme.Shared;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.VirtualFileSystem;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ByteLabs.FinanceServices.Payables.Web;

[DependsOn(
    typeof(PayablesApplicationAbstractionsModule),
    typeof(AspNetCoreMvcUiThemeSharedModule),
    typeof(AbpAutoMapperModule)
    )]
public class PayablesWebModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<MvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(PayablesResource), typeof(PayablesWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(PayablesWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AspNetCoreNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new PayablesMenuContributor());
        });

        Configure<VirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<PayablesWebModule>();
        });

        context.Services.AddAutoMapperObjectMapper<PayablesWebModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<PayablesWebModule>(validate: true);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Products/Index", PayablesPermissions.Products.Default);
        });
    }
}
