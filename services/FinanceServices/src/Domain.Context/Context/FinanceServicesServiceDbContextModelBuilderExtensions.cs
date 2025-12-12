using ByteLabs.Foundations;
using Microsoft.EntityFrameworkCore;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Domain.Context;

public static class FinanceServicesServiceDbContextModelBuilderExtensions
{
    public static void ConfigureProductService(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));


    }
}
