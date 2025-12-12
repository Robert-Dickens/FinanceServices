using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ByteLabs.FinanceServices.Payables.Domain
{
    [ConnectionStringName(PayablesDbProperties.ConnectionStringName)]
    public interface IPayablesDbContext : IEfCoreDbContext
    {
       
    }
}
