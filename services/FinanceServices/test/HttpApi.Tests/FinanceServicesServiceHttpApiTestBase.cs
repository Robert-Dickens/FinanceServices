using ByteLabs.Foundations.AspNetCore.TestFactory.Fixtures;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Testing;

[Collection(FinanceServicesServiceRemoteServiceConsts.RemoteServiceName)]
public abstract class FinanceServicesServiceHttpApiTestBase : WebApplicationFactoryIntegrationTest<Program>
{
}
