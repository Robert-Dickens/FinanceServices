using ByteLabs.FinanceServices.Services.Saas.Testing;
using ByteLabs.Foundations.AspNetCore.TestFactory;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<SaasServiceHttpApiTestModule>();

namespace ByteLabs.FinanceServices.Services.Saas.Testing
{
    public partial class Program
    {
    }
}
