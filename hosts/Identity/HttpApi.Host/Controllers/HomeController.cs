using Microsoft.AspNetCore.Mvc;
using ByteLabs.Foundations.AspNetCore.Mvc;

namespace FinanceServices.Services.IdentityService.Controllers;

public class HomeController : AspNetCoreController
{
    public ActionResult Index()
    {
        return Redirect("/swagger");
    }
}
