using ByteLabs.Foundations.AspNetCore.TestFactory.Fixtures;

namespace ByteLabs.FinanceServices.Payables.Testing;

[Collection(PayablesRemoteServiceConsts.RemoteServiceName)]
public abstract class PayablesHttpApiTestBase : WebApplicationFactoryIntegrationTest<Program>
{
}
