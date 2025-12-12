using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.PlatformServices.Identity.Domain;
using ByteLabs.PlatformServices.Security.IdentityServer.Domain.Context;

namespace ByteLabs.FinanceServices.Services.Identity.Domain
{
    [ConnectionStringName(IdentityServiceDbProperties.ConnectionStringName)]
    public interface IIdentityServiceDbContext : IEfCoreDbContext, IIdentityDbContext, IIdentityServerDbContext
    {
    }
}
