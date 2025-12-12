using ByteLabs.Aps.Hosting.Blazor;
using ByteLabs.FinanceServices.Services.Administration.Blazor;
using ByteLabs.FinanceServices.Services.Saas.Blazor;
using ByteLabs.Foundations;
using ByteLabs.Foundations.AspNetCore;
using ByteLabs.Foundations.AspNetCore.Blazor;
using ByteLabs.Foundations.AspNetCore.Blazor.Bundling;
using ByteLabs.Foundations.AspNetCore.Components.Web.Routing;
using ByteLabs.Foundations.AspNetCore.Components.Web.Toolbars;
using ByteLabs.Foundations.AspNetCore.Mvc.Client;
using ByteLabs.Foundations.AspNetCore.Mvc.Localization;
using ByteLabs.Foundations.AspNetCore.Mvc.UI.Theme.Shared;
using ByteLabs.Foundations.AspNetCore.UI.Bundling;
using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.AspNetCore.Web;
using ByteLabs.Foundations.AutoMapper;
using ByteLabs.Foundations.Http.Client;
using ByteLabs.Foundations.Http.Client.IdentityModel.Web;
using ByteLabs.Foundations.Http.Client.Web;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.Security.OpenTrust;
using ByteLabs.Foundations.Security.OpenTrust.Options;
using ByteLabs.PlatformServices.Account;
using ByteLabs.PlatformServices.Account.Web.Impersonation.Web;
using ByteLabs.PlatformServices.Security.IdentityServer.Blazor.Server;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using FinanceServices.ManagementPortal.Blazor.Menus;
using FinanceServices.ManagementPortal.Blazor.WebAssembly;
using FinanceServices.ManagementPortal.Blazor.WebAssembly.Menus;
using FinanceServices.Shared;
using FinanceServices.Shared.Hosting.AspNetCore;
using FinanceServices.Shared.Localization;
using Prometheus;

namespace FinanceServices.ManagementPortal.Blazor;

[DependsOn(
    typeof(AspNetCoreHostingModule),
    typeof(AspNetCoreMvcClientModule),
    typeof(HttpClientIdentityModelWebModule),
    typeof(HttpClientWebModule),
    typeof(AspNetCoreFluentDesignBlazorThemeModule),
    typeof(AccountPublicHttpApiClientModule),
    typeof(AdministrationServiceHttpApiClientModule),
    typeof(IdentityServiceHttpApiClientModule),
    typeof(SaasServiceHttpApiClientModule),
    typeof(FinanceServicesServiceHttpApiClientModule),
    typeof(SaasServiceBlazorServerModule),
    typeof(IdentityServiceBlazorServerModule),
    typeof(AdministrationServiceBlazorServerModule),
    typeof(IdentityServerBlazorServerModule),
    typeof(FinanceServicesServiceBlazorServerModule),
    typeof(AspNetCoreWebAccountImpersonationModule),
    typeof(SharedLocalizationModule),
    typeof(PlatformHostingBlazorModule)
   )]
public class FinanceServicesBlazorWebAppModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        context.Services.PreConfigure<MvcDataAnnotationsLocalizationOptions>(options =>
        {

            options.AddAssemblyResource(
            typeof(FinanceServicesResource),
                typeof(FinanceServicesBlazorWebAppModule).Assembly
            );
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

    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;


        context.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

        // Add services to the container.
        context.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        Configure<BlazorRenderModeOptions>(options =>
        {
            options.DefaultRenderMode = new InteractiveAutoRenderMode(true);
            options.SupportedRenderingModes = SupportedRenderingModes.InteractiveServer | SupportedRenderingModes.InteractiveWebAssembly;
        });

        ConfigureThemeOptions();
        ConfigureBundles();
        ConfigureAutoMapper();
        ConfigureRouter(context);
        ConfigureMenu(configuration);
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddCascadingValue("remote_services", op =>
        {
            return op.GetRequiredService<IOptions<RemoteServiceOptions>>().Value;
        });
    }

    private void ConfigureThemeOptions()
    {
        Configure<FluentDesignThemeOptions>(options =>
        {
        });
    }

    private void ConfigureBundles()
    {
        Configure<AspNetCoreBundlingOptions>(options =>
        {
            options.Parameters.InteractiveAuto = true;
            options.StyleBundles.Configure(BlazorFluentDesignThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/blazor-global-styles.css", "/ManagementPortal.Blazor.Client.styles.css");
                }
            );
        });

    }


    private void ConfigureMenu(IConfiguration configuration)
    {
        Configure<AspNetCoreNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new FinanceServicesMenuContributor(configuration));
        });

        Configure<AspNetCoreToolbarOptions>(options =>
        {
            options.Contributors.Add(new FinanceServicesToolbarContributor());
        });
    }

    private void ConfigureRouter(ServiceConfigurationContext context)
    {
        Configure<AspNetCoreRouterOptions>(options =>
        {
            options.AppAssembly = typeof(FinanceServicesBlazorWebAppModule).Assembly;
            options.AdditionalAssemblies.Add(typeof(FinanceServicesBlazorWebAssemblyModule).Assembly);
        });
    }

    private void ConfigureAutoMapper()
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<FinanceServicesBlazorWebAppModule>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var env = context.GetEnvironment();
        var app = context.GetApplicationBuilder();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseWebAssemblyDebugging();
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
        app.MapEmeddedStaticAssets();

        app.UseRouting();
        app.UseHttpMetrics();
        app.UseAuthentication();

        app.UseMultiTenancy();

        //app.UseDynamicClaims();
        app.UseAntiforgery();
        app.UseAbpSerilogEnrichers();

        app.UseAuthorization();

        app.UseConfiguredEndpoints(builder =>
        {
            builder.MapMetrics();
            builder.MapRazorComponents<FinanceServices.ManagementPortal.Blazor.Components.App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(builder.ServiceProvider.GetRequiredService<IOptions<AspNetCoreRouterOptions>>().Value.AdditionalAssemblies.ToArray());
        });
    }
}
