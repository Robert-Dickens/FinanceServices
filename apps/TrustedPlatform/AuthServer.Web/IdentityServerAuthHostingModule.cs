using ByteLabs.Foundations;
using ByteLabs.Foundations.BlobStoring;
using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.Security.IdentityServer.AspNetCore;
using ByteLabs.PlatformServices.Security.IdentityServer.AspNetCore.Account;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;

namespace FinanceServices.AuthServer.Web;

[DependsOn(
    typeof(AccountWebIdentityServerModule),
    typeof(IdentityServerAspNetCoreModule)
    )]
public class IdentityServerAuthHostingModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        IWebHostEnvironment hostingEnvironment = context.Services.GetHostingEnvironment();
        IConfiguration configuration = context.Services.GetConfiguration();

        PreConfigure<AuthorizationOptions>(options =>
        {
            options.AddPolicy("TwoFactorEnabled", x => x.RequireClaim("amr", "mfa"));
        });

        PreConfigure<PlatformIdentityServerBuilderOptions>(options =>
        {
            options.AddDeveloperSigningCredential = hostingEnvironment.IsDevelopment();
        });


        if (!hostingEnvironment.IsDevelopment())
        {
            PreConfigure<IIdentityServerBuilder>(identityServerBuilder =>
            {
                identityServerBuilder.AddSigningCredential(GetSigningCertificate(hostingEnvironment, configuration));
            });
        }
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        IConfiguration configuration = context.Services.GetConfiguration();
        IWebHostEnvironment hostingEnvironment = context.Services.GetHostingEnvironment();
        ConfigureAuthentication(context, configuration, hostingEnvironment);

        Configure<AccountIdentityServerOptions>(options =>
        {
            options.ClientIdToDeviceMap.Add("clientId", "device");
        });

    }

    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
    {
        string? selfUrl = configuration["App:SelfUrl"];
        string? tokenIssuerUrl = configuration["App:TokenIssuerUrl"] ?? configuration["App:SelfUrl"];
        string appName = context.Services.GetApplicationName() ?? hostingEnvironment.ApplicationName;

        List<string> validIssuers = new()
        {
           selfUrl
        };

        validIssuers.AddIfNotContains(tokenIssuerUrl);

        if (Convert.ToBoolean(configuration.GetValue("AuthServer:SetSelfAsIssuer", true)))
        {
            Configure<IdentityServerOptions>(options => { options.IssuerUri = tokenIssuerUrl; });
        }
        else
        {
            Configure<IdentityServerOptions>(options => { options.IssuerUri = configuration["AuthServer:Authority"]; });
        }

        context.Services.Configure<JwtBearerOptions>(options =>
        {
            options.Authority = tokenIssuerUrl;
            options.BackchannelHttpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            options.Audience = appName;
            options.TokenValidationParameters.ValidIssuers = validIssuers;

        });


        context.Services.ForwardIdentityAuthenticationForBearer();
    }

    private X509Certificate2 GetSigningCertificate(IWebHostEnvironment hostingEnv, IConfiguration configuration)
    {
        string fileName = "authserver.pfx";
        string passPhrase = "2D7AA457-5D33-48D6-936F-C48E5EF468ED";

        IConfigurationSection cerConfig = configuration.GetSection("Certificates");
        if (cerConfig.Exists())
        {
            string cerPath = Path.Combine(hostingEnv.ContentRootPath, cerConfig["CerPath"]);
            if (File.Exists(cerPath))
            {
                fileName = Path.GetFileName(cerPath);
                passPhrase = cerConfig.GetValue("Password", passPhrase);
            }
        }

        string file = Path.Combine(hostingEnv.ContentRootPath, fileName);

        return !File.Exists(file)
            ? throw new FileNotFoundException($"Signing Certificate couldn't found: {file}")
            : new X509Certificate2(file, passPhrase);
    }

}
