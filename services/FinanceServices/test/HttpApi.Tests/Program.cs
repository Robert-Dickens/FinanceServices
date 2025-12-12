using ByteLabs.FinanceServices.Services.FinanceServices.Testing;
using ByteLabs.Foundations.AspNetCore.TestFactory;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<FinanceServicesServiceHttpApiTestModule>();

namespace ByteLabs.FinanceServices.Services.FinanceServices.Testing
{
    public partial class Program
    {
    }
}
