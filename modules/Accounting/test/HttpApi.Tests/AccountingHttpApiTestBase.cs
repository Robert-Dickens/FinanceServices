using ByteLabs.Foundations.AspNetCore.TestFactory.Fixtures;

namespace ByteLabs.FinanceServices.Accounting.Testing;

[Collection(AccountingRemoteServiceConsts.RemoteServiceName)]
public abstract class AccountingHttpApiTestBase : WebApplicationFactoryIntegrationTest<Program>
{
}
