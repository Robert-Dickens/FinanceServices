using ByteLabs.FinanceServices.Payables.Testing;
using ByteLabs.Foundations.AspNetCore.TestFactory;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<PayablesHttpApiTestModule>();

namespace ByteLabs.FinanceServices.Payables.Testing
{
    public partial class Program
    {
    }
}
