using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ByteLabs.FinanceServices.Recevables.Domain.Context;

[ConnectionStringName(RecevablesDbProperties.ConnectionStringName)]
public class RecevablesDbContext : PlatformDbContext<RecevablesDbContext>, IRecevablesDbContext
{
   

    public RecevablesDbContext(DbContextOptions<RecevablesDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureRecevables();
    }
}
