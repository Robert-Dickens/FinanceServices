using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Domain.Context;

[ConnectionStringName(FinanceServicesServiceDbProperties.ConnectionStringName)]
public class FinanceServicesServiceDbContext : PlatformDbContext<FinanceServicesServiceDbContext>, IFinanceServicesServiceDbContext
{
   

    public FinanceServicesServiceDbContext(DbContextOptions<FinanceServicesServiceDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureProductService();
    }
}
