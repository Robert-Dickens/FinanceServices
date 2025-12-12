namespace ByteLabs.FinanceServices.Services.Administration.Testing.Context;

/* This class can be used as a base class for EF Core integration tests,
 * while SampleRepository_Tests uses a different approach.
 */
public abstract class AdministrationServiceDomainContextTestBase : AdministrationServiceTestBase<AdministrationServiceDomainContextTestModule>
{

}
