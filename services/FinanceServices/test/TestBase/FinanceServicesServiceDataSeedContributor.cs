using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.DependencyInjection;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Testing;

public class FinanceServicesServiceDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly FinanceServicesServiceTestData _testData;

    public FinanceServicesServiceDataSeedContributor(
        FinanceServicesServiceTestData testData)
    {
        _testData = testData;
    }

    public Task SeedAsync(DataSeedContext context)
    {
        return Task.CompletedTask;
    }
}
