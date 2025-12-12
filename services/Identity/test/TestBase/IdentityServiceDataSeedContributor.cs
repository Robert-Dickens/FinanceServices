using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.Identity.Testing;

public class IdentityServiceDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    public Task SeedAsync(DataSeedContext context)
    {
        /* Seed additional test data... */

        return Task.CompletedTask;
    }
}
