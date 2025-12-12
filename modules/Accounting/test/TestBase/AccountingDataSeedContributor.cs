using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.DependencyInjection;

namespace ByteLabs.FinanceServices.Accounting.Testing;

public class AccountingDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly AccountingTestData _testData;

    public AccountingDataSeedContributor(
        AccountingTestData testData)
    {
        _testData = testData;
    }

    public Task SeedAsync(DataSeedContext context)
    {
        return Task.CompletedTask;
    }

 
}
