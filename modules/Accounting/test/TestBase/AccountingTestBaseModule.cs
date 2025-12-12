using ByteLabs.FinanceServices.Accounting.Domain;
using ByteLabs.Foundations;
using ByteLabs.Foundations.Auditing;
using ByteLabs.Foundations.Authorization;
using ByteLabs.Foundations.Autofac;
using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.TestFactory;
using ByteLabs.Foundations.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLabs.FinanceServices.Accounting.Testing;

[DependsOn(
    typeof(AutofacModule),
    typeof(TestBaseModule),
    typeof(PlatformAuthorizationModule),
    typeof(AuditingModule),
    typeof(AccountingDomainModule)
)]
public class AccountingTestBaseModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAlwaysAllowAuthorization();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        SeedTestData(context);
    }

    private static void SeedTestData(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(async () =>
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                await scope.ServiceProvider
                    .GetRequiredService<IDataSeeder>()
                    .SeedAsync();
            }
        });
    }
}
