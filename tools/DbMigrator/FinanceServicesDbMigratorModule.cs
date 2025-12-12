using ByteLabs.FinanceServices.Services.Administration;
using ByteLabs.FinanceServices.Services.Administration.Domain.Context.PostgreSql;
using ByteLabs.FinanceServices.Services.FinanceServices;
using ByteLabs.FinanceServices.Services.FinanceServices.Domain.Context.PostgreSql;
using ByteLabs.FinanceServices.Services.Identity;
using ByteLabs.FinanceServices.Services.Identity.Domain;
using ByteLabs.FinanceServices.Services.Identity.Domain.Context.PostgreSql;
using ByteLabs.FinanceServices.Services.Saas;
using ByteLabs.FinanceServices.Services.Saas.Domain.Context.PostgreSql;
using ByteLabs.Foundations.Modularity;
using FinanceServices.Shared.Hosting;


namespace DbMigrator;

[DependsOn(
    typeof(SharedHostingModule),
    typeof(IdentityServiceDomainModule),
    typeof(AdministrationServiceApplicationAbstractionsModule),
    typeof(IdentityServiceApplicationAbstractionsModule),
    typeof(SaasServiceApplicationAbstractionsModule),
    typeof(FinanceServicesServiceApplicationAbstractionsModule)
)]
[DependsOn(typeof(SaasPostgreSqlDomainContextModule), typeof(AdministrationPostgreSqlDomainContextModule), typeof(IdentityPostgreSqlDomainContextModule), typeof(FinanceServicesServicePostgreSqlDomainContextModule))]
public class FinanceServicesDbMigratorModule : PlatformModule
{
}
