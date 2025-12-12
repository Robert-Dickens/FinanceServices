using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ByteLabs.FinanceServices.Recevables.Domain
{
    [ConnectionStringName(RecevablesDbProperties.ConnectionStringName)]
    public interface IRecevablesDbContext : IEfCoreDbContext
    {
    }
}
