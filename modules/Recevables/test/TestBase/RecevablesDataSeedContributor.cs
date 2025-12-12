using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.DependencyInjection;

namespace ByteLabs.FinanceServices.Recevables.Testing;

public class RecevablesDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly RecevablesTestData _testData;

    public RecevablesDataSeedContributor(
        RecevablesTestData testData)
    {
        _testData = testData;
    }

    public Task SeedAsync(DataSeedContext context)
    {
        return Task.CompletedTask;
    }


}
