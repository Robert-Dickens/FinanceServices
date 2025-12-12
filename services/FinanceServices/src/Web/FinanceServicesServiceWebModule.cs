using ByteLabs.FinanceServices.Services.FinanceServices.Permissions;
using ByteLabs.FinanceServices.Services.FinanceServices.Web.Menus;
using ByteLabs.Foundations.AspNetCore.Mvc.Localization;
using ByteLabs.Foundations.AspNetCore.Mvc.UI.Theme.Shared;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.VirtualFileSystem;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Web;

[DependsOn(
    typeof(FinanceServicesServiceApplicationAbstractionsModule),
    typeof(AspNetCoreMvcUiThemeSharedModule),
    typeof(AbpAutoMapperModule),
    typeof(FinanceServicesServiceHttpApiModule)
    )]
public class FinanceServicesServiceWebModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<MvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(FinanceServicesServiceResource), typeof(FinanceServicesServiceWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(FinanceServicesServiceWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AspNetCoreNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new FinanceServicesServiceMenuContributor());
        });

        Configure<VirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<FinanceServicesServiceWebModule>();
        });

        context.Services.AddAutoMapperObjectMapper<FinanceServicesServiceWebModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<FinanceServicesServiceWebModule>(validate: true);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Products/Index", FinanceServicesServicePermissions.Products.Default);
        });
    }
}
