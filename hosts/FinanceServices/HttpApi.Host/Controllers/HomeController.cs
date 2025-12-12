using ByteLabs.Foundations.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace FinanceServices.Services.FinanceServicesService.Controllers;

public class HomeController : AspNetCoreController
{
    public ActionResult Index()
    {
        return Redirect("/swagger");
    }
}
