using ByteLabs.Aps.Hosting.MVC;
using ByteLabs.FinanceServices.Services.FinanceServices.Web;
using ByteLabs.Foundations;
using ByteLabs.Foundations.AspNetCore;
using ByteLabs.Foundations.AspNetCore.Mvc.Client;
using ByteLabs.Foundations.AspNetCore.Mvc.Localization;
using ByteLabs.Foundations.AspNetCore.Mvc.UI;
using ByteLabs.Foundations.AspNetCore.Mvc.UI.Theme.Shared;
using ByteLabs.Foundations.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using ByteLabs.Foundations.AspNetCore.Mvc.UI.Theming;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.Authentication.OpenIdConnect;
using ByteLabs.Foundations.Http.Client.Web;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.Security.OpenTrust;
using ByteLabs.Foundations.Security.OpenTrust.Options;
using ByteLabs.PlatformServices.Account;
using ByteLabs.PlatformServices.Account.Web.Impersonation.Web;
using ByteLabs.PlatformServices.Gdpr.Web;
using ByteLabs.PlatformServices.Gdpr.Web.Extensions;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using FinanceServices.PublicServer.Web.Menus;
using FinanceServices.Shared;
using FinanceServices.Shared.Hosting.AspNetCore;
using FinanceServices.Shared.Localization;
using Prometheus;

namespace FinanceServices.PublicServer.Web;

[DependsOn(
    typeof(AspNetCoreHostingModule),
    typeof(PlatformHostingMvcModule),
    typeof(AspNetCoreMvcClientModule),
    typeof(AspNetCoreMvcUiMultiTenancyModule),
    typeof(HttpClientWebModule),
    typeof(AccountPublicHttpApiClientModule),
    typeof(FinanceServicesServiceWebModule),
    typeof(FinanceServicesServiceHttpApiClientModule),
    typeof(AdministrationServiceHttpApiClientModule),
    typeof(AspNetCoreWebAccountImpersonationModule),
    typeof(GdprWebModule),
    typeof(SharedLocalizationModule)
)]
public class FinanceServicesPublicWebModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        IConfiguration configuration = context.Services.GetConfiguration();

        context.Services.PreConfigure<MvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(FinanceServicesResource),
                typeof(FinanceServicesPublicWebModule).Assembly
            );
        });

        PreConfigure<TokenIntrospectionOptions>(options =>
        {
            options.UseTokenIntrospection = false;
        });

        PreConfigure<SecuritySection>(options =>
        {
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            options.ConfigureAuthentication(provider =>
            {
                provider.AuthorizationType = AuthorizationType.OpenIdConnect;
                provider.Authority = configuration["AuthServer:Authority"];
                provider.RequireHttpsMetadata = true;
                provider.ResponseType = OpenIdConnectResponseType.Code;

                provider.ClientId = configuration["AuthServer:ClientId"];
                provider.ClientSecret = configuration["AuthServer:ClientSecret"];

                provider.SaveTokens = true;
                provider.GetClaimsFromUserInfoEndpoint = true;

                provider.AllowOfflineAccess = true;

                provider.Scope.Add("role");
                provider.Scope.Add("email");
                provider.Scope.Add("phone");
                provider.Scope.Add("offline_access");
                provider.Scope.Add(GlobalConstants.Services.AccountServiceName);
                provider.Scope.Add(GlobalConstants.Services.AdministrationServiceName);
                provider.Scope.Add(GlobalConstants.Services.FinanceServicesServiceName);
            });
        });

    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        IWebHostEnvironment hostingEnvironment = context.Services.GetHostingEnvironment();
        IConfiguration configuration = context.Services.GetConfiguration();


        context.Services.AddAbpCookieConsent(options =>
        {
            options.IsEnabled = true;
        });


        Configure<AspNetCoreNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new FinanceServicesPublicWebMenuContributor(configuration));
        });

        Configure<AppToolbarOptions>(options =>
        {
            options.Contributors.Add(new FinanceServicesPublicWebToolbarContributor());
        });

        Configure<AspNetCoreThemingOptions>(options =>
        {
            options.IsPublicWebsite = true;
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        IApplicationBuilder app = context.GetApplicationBuilder();
        IWebHostEnvironment env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseAbpRequestLocalization();

        app.UseRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.UseAbpSecurityHeaders();
        app.UseStaticFiles();

        app.UseAbpCookieConsent();

        app.UseRouting();
        app.UseHttpMetrics();
        app.UseAuthentication();
        app.UseMultiTenancy();
        app.UseAbpSerilogEnrichers();
        app.UseAuthorization();
        app.UseConfiguredEndpoints(endpoints =>
        {
            endpoints.MapMetrics();
        });
    }
}
