using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.EntityFrameworkCore;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Domain
{
    [ConnectionStringName(FinanceServicesServiceDbProperties.ConnectionStringName)]
    public interface IFinanceServicesServiceDbContext : IEfCoreDbContext
    {
     
    }
}
