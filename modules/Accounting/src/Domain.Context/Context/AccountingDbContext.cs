using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.Data;
using Microsoft.EntityFrameworkCore;

namespace ByteLabs.FinanceServices.Accounting.Domain.Context;

[ConnectionStringName(AccountingDbProperties.ConnectionStringName)]
public class AccountingDbContext : PlatformDbContext<AccountingDbContext>, IAccountingDbContext
{

    public AccountingDbContext(DbContextOptions<AccountingDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureAccounting();
    }
}
