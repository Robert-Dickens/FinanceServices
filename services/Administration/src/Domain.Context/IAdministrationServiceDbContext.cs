using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Platform.Foundations.Domain.Context;

namespace ByteLabs.FinanceServices.Services.Administration.Domain
{
    [ConnectionStringName(AdministrationServiceDbProperties.ConnectionStringName)]
    public interface IAdministrationServiceDbContext : IEfCoreDbContext, IPlatformServicesDbContext
    {
    }
}
