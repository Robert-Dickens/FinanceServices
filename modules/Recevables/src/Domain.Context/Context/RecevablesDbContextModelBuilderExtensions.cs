using ByteLabs.Foundations;
using ByteLabs.Foundations.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore;

namespace ByteLabs.FinanceServices.Recevables.Domain.Context;

public static class RecevablesDbContextModelBuilderExtensions
{
    public static void ConfigureRecevables(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));


    }
}
