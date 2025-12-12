using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.PlatformServices.SaaS.Domain.Context;
using ByteLabs.PlatformServices.SaaS.Domain.Editions;
using ByteLabs.PlatformServices.SaaS.Domain.Tenants;
using Microsoft.EntityFrameworkCore;

namespace ByteLabs.FinanceServices.Services.Saas.Domain.Context;

[ConnectionStringName(SaasServiceDbProperties.ConnectionStringName)]
public class SaasServiceDbContext : PlatformDbContext<SaasServiceDbContext>, ISaasServiceDbContext, ISaasDbContext
{

    public SaasServiceDbContext(DbContextOptions<SaasServiceDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tenant> Tenants { get; protected set; }

    public DbSet<Edition> Editions { get; protected set; }

    public DbSet<TenantConnectionString> TenantConnectionStrings { get; protected set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if(modelBuilder.IsHostDatabase())
        {
            modelBuilder.ConfigureSaas();
        }
    }
}
