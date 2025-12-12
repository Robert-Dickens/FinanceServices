using ByteLabs.Foundations.AspNetCore.TestFactory.Fixtures;

namespace ByteLabs.FinanceServices.Services.Administration.Testing;

[Collection(AdministrationServiceRemoteServiceConsts.RemoteServiceName)]
public abstract class AdministrationServiceHttpApiTestBase : WebApplicationFactoryIntegrationTest<Program>
{
}
