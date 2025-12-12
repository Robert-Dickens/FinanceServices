namespace ByteLabs.FinanceServices.Payables.Testing.Context;

/* This class can be used as a base class for EF Core integration tests,
 * while ProductServiceRepositoryTests uses a different approach.
 */
public abstract class PayablesServiceDomainContextTestBase : PayablesTestBase<PayablesDomainContextTestModule>
{

}
