using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ByteLabs.FinanceServices.Payables.Domain.Context;

[ConnectionStringName(PayablesDbProperties.ConnectionStringName)]
public class PayablesDbContext : PlatformDbContext<PayablesDbContext>, IPayablesDbContext
{
   

    public PayablesDbContext(DbContextOptions<PayablesDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigurePayables();
    }
}
