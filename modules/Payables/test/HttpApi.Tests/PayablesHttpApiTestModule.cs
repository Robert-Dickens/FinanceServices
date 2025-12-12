using ByteLabs.Foundations;
using ByteLabs.Foundations.AspNetCore;
using ByteLabs.Foundations.AspNetCore.Mvc;
using ByteLabs.Foundations.AspNetCore.TestFactory;
using ByteLabs.Foundations.AspNetCore.TestFactory.Authentication;
using ByteLabs.Foundations.DistributedSystems.DistributedLocking.FileSystem;
using ByteLabs.Foundations.Http.Client;
using ByteLabs.Foundations.Modularity;

namespace ByteLabs.FinanceServices.Payables.Testing;

[DependsOn(
    typeof(DistributedLockingFileSystemModule),
    typeof(HttpClientModule),
    typeof(AspNetCoreTestFactoryApiModule),
    typeof(PayablesApplicationTestModule),
    typeof(PayablesHttpApiModule)
    )]
public class PayablesHttpApiTestModule : PlatformModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(PayablesApplicationModule).Assembly, opts =>
            {
                opts.ApplicationServiceTypes = ApplicationServiceTypes.All;
            });
        });

        PreConfigure<ClaimInjectorHandlerHeaderConfig>(options =>
        {
            options.AnonymousRequest = false;
        });

    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var preActions = context.Services.GetPreConfigureActions<AspNetCoreMvcOptions>();
        Configure<AspNetCoreMvcOptions>(options =>
        {
            preActions.Configure(options);
        });

        context.Services.AddHttpClientProxies(typeof(PayablesHttpApiTestModule).Assembly);

        Configure<RemoteServiceOptions>(options =>
        {
            options.RemoteServices.Default = new RemoteServiceConfiguration("/");
        });

    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseConfiguredEndpoints();
    }
}