using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.DependencyInjection;

namespace ByteLabs.FinanceServices.Payables.Testing;

public class PayablesDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly PayablesTestData _testData;

    public PayablesDataSeedContributor(
        PayablesTestData testData)
    {
        _testData = testData;
    }

    public Task SeedAsync(DataSeedContext context)
    {
        return Task.CompletedTask;
    }

}
