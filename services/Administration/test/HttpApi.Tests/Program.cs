using ByteLabs.FinanceServices.Services.Administration.Testing;
using ByteLabs.Foundations.AspNetCore.TestFactory;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<AdministrationServiceHttpApiTestModule>();

namespace ByteLabs.FinanceServices.Services.Administration.Testing
{
    public partial class Program
    {
    }
}
