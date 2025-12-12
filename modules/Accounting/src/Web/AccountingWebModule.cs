using ByteLabs.Foundations.AspNetCore.Mvc.Localization;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.VirtualFileSystem;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ByteLabs.FinanceServices.Accounting.Localization;
using ByteLabs.FinanceServices.Accounting.Permissions;
using ByteLabs.FinanceServices.Accounting.Web.Menus;
using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.AspNetCore.Mvc.UI.Theme.Shared;

namespace ByteLabs.FinanceServices.Accounting.Web;

[DependsOn(
    typeof(AccountingApplicationAbstractionsModule),
    typeof(AspNetCoreMvcUiThemeSharedModule),
    typeof(AbpAutoMapperModule)
    )]
public class AccountingWebModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<MvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(AccountingResource), typeof(AccountingWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AccountingWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AspNetCoreNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AccountingMenuContributor());
        });

        Configure<VirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AccountingWebModule>();
        });

        context.Services.AddAutoMapperObjectMapper<AccountingWebModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AccountingWebModule>(validate: true);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Products/Index", AccountingPermissions.Products.Default);
        });
    }
}
