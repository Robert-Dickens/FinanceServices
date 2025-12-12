using ByteLabs.FinanceServices.Recevables.Testing;
using ByteLabs.Foundations.AspNetCore.TestFactory;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<RecevablesHttpApiTestModule>();

namespace ByteLabs.FinanceServices.Recevables.Testing
{
    public partial class Program
    {
    }
}
