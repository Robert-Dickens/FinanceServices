using ByteLabs.FinanceServices.Accounting.Testing;
using ByteLabs.Foundations.AspNetCore.TestFactory;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<AccountingHttpApiTestModule>();

namespace ByteLabs.FinanceServices.Accounting.Testing
{
    public partial class Program
    {
    }
}
