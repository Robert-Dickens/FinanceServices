using ByteLabs.FinanceServices.Services.Identity.Testing;
using ByteLabs.Foundations.AspNetCore.TestFactory;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<IdentityServiceHttpApiTestModule>();

namespace ByteLabs.FinanceServices.Services.Identity.Testing
{
    public partial class Program
    {
    }
}
