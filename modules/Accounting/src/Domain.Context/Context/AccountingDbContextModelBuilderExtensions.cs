using ByteLabs.Foundations.EntityFrameworkCore.Modeling;
using ByteLabs.Foundations;
using Microsoft.EntityFrameworkCore;

namespace ByteLabs.FinanceServices.Accounting.Domain.Context;

public static class AccountingDbContextModelBuilderExtensions
{
    public static void ConfigureAccounting(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));


    }
}
