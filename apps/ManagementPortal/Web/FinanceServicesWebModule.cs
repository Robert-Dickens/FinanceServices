using ByteLabs.Aps.Hosting.MVC;
using ByteLabs.FinanceServices.Services.Administration.Web;
using ByteLabs.FinanceServices.Services.FinanceServices.Web;
using ByteLabs.FinanceServices.Services.Identity.Web;
using ByteLabs.FinanceServices.Services.Saas.Web;
using ByteLabs.Foundations;
using ByteLabs.Foundations.AspNetCore;
using ByteLabs.Foundations.AspNetCore.Mvc.Client;
using ByteLabs.Foundations.AspNetCore.Mvc.Localization;
using ByteLabs.Foundations.AspNetCore.Mvc.UI;
using ByteLabs.Foundations.AspNetCore.Mvc.UI.Theme.Shared;
using ByteLabs.Foundations.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.Authentication.OpenIdConnect;
using ByteLabs.Foundations.DistributedSystems.CAP;
using ByteLabs.Foundations.DistributedSystems.CAP.Dashboard;
using ByteLabs.Foundations.DistributedSystems.CAP.Dashboard.Permissions;
using ByteLabs.Foundations.Http.Client.Web;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.Security.OpenTrust;
using ByteLabs.Foundations.Security.OpenTrust.Options;
using ByteLabs.Foundations.VirtualFileSystem;
using ByteLabs.PlatformServices.Account;
using ByteLabs.PlatformServices.Account.Public.Web;
using ByteLabs.PlatformServices.Account.Web.Impersonation.Web;
using ByteLabs.PlatformServices.Gdpr.Web;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using FinanceServices.ManagementPortal.Web.Navigation;
using FinanceServices.Shared;
using FinanceServices.Shared.Hosting.AspNetCore;
using FinanceServices.Shared.Localization;
using Prometheus;

namespace FinanceServices.ManagementPortal.Web;

[DependsOn(
    typeof(AspNetCoreHostingModule),
    //typeof(DistributedServicesHostingModule),
    typeof(PlatformHostingMvcModule),
    typeof(AspNetCoreMvcClientModule),
    typeof(AspNetCoreMvcUiMultiTenancyModule),
    typeof(HttpClientWebModule),
    typeof(AccountPublicHttpApiClientModule),
    typeof(SaasServiceWebModule),
    typeof(SaasServiceHttpApiClientModule),
    typeof(FinanceServicesServiceWebModule),
    typeof(FinanceServicesServiceHttpApiClientModule),
    typeof(IdentityServiceWebModule),
    typeof(IdentityServiceHttpApiClientModule),
    typeof(AdministrationServiceWebModule),
    typeof(AdministrationServiceHttpApiClientModule),
    typeof(AccountPublicWebSharedModule),
    typeof(AspNetCoreWebAccountImpersonationModule),
    typeof(GdprWebModule),
    typeof(SharedLocalizationModule)
)]
public class FinanceServicesWebModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        IConfiguration configuration = context.Services.GetConfiguration();

        context.Services.PreConfigure<MvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(FinanceServicesResource),
                typeof(FinanceServicesWebModule).Assembly
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
                provider.Scope.Add(GlobalConstants.Services.IdentityServiceName);
                provider.Scope.Add(GlobalConstants.Services.SaasServiceName);
            });
        });


        //PlatformCAPEventBusOptions capOptions = context.Services.ExecutePreConfiguredActions<PlatformCAPEventBusOptions>();
        //if (capOptions.EnableDashBoard == true)
        //{
        //    PreConfigure<CapOptions>(options =>
        //    {
        //        options.UseAbpDashboard().UseClusterDiscovery(options =>
        //        {
        //            IConfiguration configuration = context.Services.GetConfiguration();
        //            options.ClusterDiscoveryUri = configuration.GetValue<string>("CAP:EventBusOptions:ClusterDiscoveryUri", $"https://{GlobalConstants.Services.GatewayServiceName}");
        //        });
        //    });
        //}

    }


    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        IWebHostEnvironment hostingEnvironment = context.Services.GetHostingEnvironment();
        IConfiguration configuration = context.Services.GetConfiguration();


        //ConfigureCapDashboardAuthenticationPolicy(context);


        Configure<AspNetCoreNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new FinanceServicesMenuContributor(configuration));
        });

        Configure<AppToolbarOptions>(options =>
        {
            options.Contributors.Add(new FinanceServicesToolbarContributor());
        });

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<FinanceServicesServiceWebModule>(Path.Combine(
                    hostingEnvironment.ContentRootPath,
                    $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}services{Path.DirectorySeparatorChar}FinanceServices{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}Web"));
            });
        }
    }

    private void ConfigureCapDashboardAuthenticationPolicy(ServiceConfigurationContext context)
    {
        context.Services.AddAuthentication()
            .AddScheme<CapDashboardAuthenticationSchemeOptions, CapDashboardAuthenticationHandler>(
                  CapDashboardAuthenticationHandler.SchemeName,
                  options =>
                  {
                      options.PermissionName = CapDashboardPermissions.Manage;
                      options.Roles = new[] { CapDashboardPermissions.DefaultAdminRole };
                  });

        Configure<AuthorizationOptions>(options =>
        {
            options.AddPolicy(PlatformEventBusCapDashboardModule.CapDashboardAuthenticationPolicy, policy => policy
                .AddAuthenticationSchemes(CapDashboardAuthenticationHandler.SchemeName, "oidc", "Cookies")
                .RequireAuthenticatedUser());
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
