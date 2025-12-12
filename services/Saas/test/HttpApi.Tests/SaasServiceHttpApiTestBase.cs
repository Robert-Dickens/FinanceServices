using ByteLabs.Foundations.AspNetCore.TestFactory.Fixtures;

namespace ByteLabs.FinanceServices.Services.Saas.Testing;

[Collection(SaasServiceRemoteServiceConsts.RemoteServiceName)]
public abstract class SaasServiceHttpApiTestBase : WebApplicationFactoryIntegrationTest<Program>
{
}
