using ByteLabs.FinanceServices;
using ByteLabs.FinanceServices.Hosting.Microservices;
using ByteLabs.FinanceServices.Localization;
using ByteLabs.FinanceServices.Services.Administration;
using ByteLabs.FinanceServices.Services.Identity;
using ByteLabs.FinanceServices.Services.Identity.Domain.Context.PostgreSql;
using ByteLabs.FinanceServices.Services.Identity.Web;
using IdentityServer4.Extensions;
using ByteLabs.Foundations.AspNetCore.Authentication.OpenIdConnect;
using ByteLabs.Foundations;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.Security.Claims;
using ByteLabs.PlatformServices.Account;
using ByteLabs.PlatformServices.Account.Admin.Web;
using ByteLabs.PlatformServices.Account.Public.Web;
using ByteLabs.PlatformServices.Account.Web.Impersonation.Web;
using ByteLabs.PlatformServices.Identity.Permissions;
using ByteLabs.PlatformServices.SaaS.Permissions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Prometheus;
using ByteLabs.Foundations.Auditing;
using ByteLabs.Foundations.AspNetCore.UI.Bundling;
using ByteLabs.Foundations.AspNetCore.Mvc.UI.Theme.Bootstrap.Bundling;
using ByteLabs.Foundations.AspNetCore.Components.Bootstrap.Razor;
using ByteLabs.Foundations.AspNetCore.Mvc.UI.Theme.Shared;
using ByteLabs.Foundations.AspNetCore.Mvc.AntiForgery;
using ByteLabs.Foundations.AspNetCore;
using ByteLabs.Foundations.BlobStoring.FileSystem;
using ByteLabs.Foundations.BlobStoring;






namespace FinanceServices.AuthServer.Web;

[DependsOn(typeof(SharedHostingMicroservicesModule))]
[DependsOn(typeof(IdentityPostgreSqlDomainContextModule))]
[DependsOn(
    typeof(AspNetCoreBootstrapRazorThemeModule),
    typeof(AccountPublicWebModule),
    typeof(AccountPublicHttpApiModule),
    typeof(AccountAdminApplicationModule),
    typeof(AccountAdminHttpApiModule),
    typeof(AccountAdminWebModule),
    typeof(IdentityServiceWebModule),
    typeof(IdentityServiceApplicationModule),
    typeof(AdministrationServiceApplicationModule),
    typeof(IdentityServerAuthHostingModule),
    typeof(SharedLocalizationModule),
    typeof(AspNetCoreWebAccountImpersonationModule),
    typeof(AspNetCoreAuthenticationSocialOpenIdConnectModule),
    typeof(BlobStoringFileSystemModule)
)]
public class FinanceServicesAuthServerModule : PlatformModule
{

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        IWebHostEnvironment hostingEnvironment = context.Services.GetHostingEnvironment();
        IConfiguration configuration = context.Services.GetConfiguration();
        string appName = context.Services.GetApplicationName() ?? hostingEnvironment.ApplicationName;

        Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = !configuration.GetValue("App:ShowPII", false);

        Configure<AuditingOptions>(options =>
        {
            options.IsEnabledForGetRequests = true;
            options.ApplicationName = appName;
        });

        Configure<AspNetCoreAntiForgeryOptions>(options =>
        {
            options.AutoValidate = false;
        });

        context.Services.Configure<IISServerOptions>(options =>
        {
            options.MaxRequestBodySize = int.MaxValue;
        });
        context.Services.Configure<FormOptions>(options =>
        {
            options.ValueLengthLimit = int.MaxValue;
            options.MultipartBodyLengthLimit = int.MaxValue;
            options.MultipartHeadersLengthLimit = int.MaxValue;
        });
        context.Services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = int.MaxValue;
        });

        ConfigureImpersonation(context);


        ConfigureBundles();

        context.Services.Configure<ClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });

        Configure<BlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                container.UseFileSystem(fileSystem =>
                {
                    fileSystem.BasePath = context.Services.GetHostingEnvironment().WebRootPath;
                });
            });
        });

    }

    private void ConfigureImpersonation(ServiceConfigurationContext context)
    {
        context.Services.Configure<AccountOptions>(options =>
        {
            options.TenantAdminUserName = "admin";
            options.ImpersonationTenantPermission = AbpSaasPermissions.Tenants.Impersonation;
            options.ImpersonationUserPermission = IdentityPermissions.Users.Impersonation;
        });
    }

    private void ConfigureBundles()
    {
        Configure<AspNetCoreBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                BootstrapRazorThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });
    }


    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        IApplicationBuilder app = context.GetApplicationBuilder();
        IWebHostEnvironment env = context.GetEnvironment();

        IConfiguration configuration = context.GetConfiguration();
        (bool isEnabled, bool isUiEnabled) openApiContext = context.GetOpenApiSwaggerContext();

        app.Use(async (ctx, next) =>
        {
            if (ctx.Request.Headers.ContainsKey("from-ingress"))
            {
                ctx.Request.Scheme = "https";
                ctx.SetIdentityServerOrigin(configuration["App:SelfUrl"]);
            }

            await next();
        });

        app.UseAbpRequestLocalization();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.UseAbpSecurityHeaders();
        app.UseStaticFiles();

        app.UseCookiePolicy();

        ForwardedHeadersOptions fordwardedHeaderOptions = new()
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        };
        fordwardedHeaderOptions.KnownNetworks.Clear();
        fordwardedHeaderOptions.KnownProxies.Clear();
        app.UseForwardedHeaders(fordwardedHeaderOptions);


        app.UseRouting();
        app.UseCors();

        app.UseHttpMetrics();
        app.UseAuthentication();
        app.UseMultiTenancy();
        app.UseUnitOfWork();

        app.UseIdentityServer();
        app.UseDynamicClaims();
        app.UseAuthorization();

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints(endpoints =>
        {
            endpoints.MapMetrics();
        });
        if (openApiContext.isEnabled)
            app.UseSwagger();
        if (openApiContext.isUiEnabled)
            app.UseOpenApiSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{GlobalConstants.Services.AccountServiceName} API");
                options.OAuthClientId(configuration["OpenApi:SwaggerClientId"]);
                options.OAuthClientSecret(configuration["OpenApi:SwaggerClientSecret"]);
            });
    }

}
