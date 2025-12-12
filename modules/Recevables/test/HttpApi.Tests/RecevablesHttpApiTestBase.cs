using ByteLabs.Foundations.AspNetCore.TestFactory.Fixtures;

namespace ByteLabs.FinanceServices.Recevables.Testing;

[Collection(RecevablesRemoteServiceConsts.RemoteServiceName)]
public abstract class RecevablesHttpApiTestBase : WebApplicationFactoryIntegrationTest<Program>
{
}
