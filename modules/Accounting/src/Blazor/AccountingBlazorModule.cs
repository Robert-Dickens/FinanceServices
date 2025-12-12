using ByteLabs.Foundations.Modularity;
using Microsoft.Extensions.DependencyInjection;
using ByteLabs.FinanceServices.Accounting.Blazor.Menus;
using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.AspNetCore.Components.Web;
using ByteLabs.Foundations.AspNetCore.Components.Web.Routing;

namespace ByteLabs.FinanceServices.Accounting.Blazor;

[DependsOn(
    typeof(AccountingApplicationAbstractionsModule),
    typeof(AspNetCoreComponentsWebThemingModule),
    typeof(AbpAutoMapperModule)
    )]
public class AccountingBlazorModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AccountingBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AccountingBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AspNetCoreNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AccountingMenuContributor());
        });

        Configure<AspNetCoreRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(AccountingBlazorModule).Assembly);
        });
    }
}
