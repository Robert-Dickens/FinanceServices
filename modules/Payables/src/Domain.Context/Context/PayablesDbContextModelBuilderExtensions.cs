using ByteLabs.Foundations;
using ByteLabs.Foundations.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore;

namespace ByteLabs.FinanceServices.Payables.Domain.Context;

public static class PayablesDbContextModelBuilderExtensions
{
    public static void ConfigurePayables(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
        
    }
}
