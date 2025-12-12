using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.PlatformServices.SaaS.Domain.Context;

namespace ByteLabs.FinanceServices.Services.Saas.Domain
{
    [ConnectionStringName(SaasServiceDbProperties.ConnectionStringName)]
    public interface ISaasServiceDbContext : IEfCoreDbContext, ISaasDbContext
    {
    }
}
