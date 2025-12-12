using ByteLabs.Foundations.AspNetCore.TestFactory.Fixtures;

namespace ByteLabs.FinanceServices.Services.Identity.Testing;

[Collection(IdentityServiceRemoteServiceConsts.RemoteServiceName)]
public abstract class IdentityServiceHttpApiTestBase : WebApplicationFactoryIntegrationTest<Program>
{
}
