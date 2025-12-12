using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.Data;
using Microsoft.EntityFrameworkCore;

namespace ByteLabs.FinanceServices.Accounting.Domain
{
    [ConnectionStringName(AccountingDbProperties.ConnectionStringName)]
    public interface IAccountingDbContext : IEfCoreDbContext
    {
    }
}
