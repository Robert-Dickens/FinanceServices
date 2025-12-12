using Microsoft.AspNetCore.Mvc;
using ByteLabs.Foundations.AspNetCore.Mvc.UI.RazorPages;

namespace FinanceServices.AuthServer.Web.Pages;

public class IndexModel : AspNetCorePageModel
{
    public ActionResult OnGet()
    {
        if (!CurrentUser.IsAuthenticated)
        {
            return Redirect("~/Account/Login");
        }
        else
        {
            return Page();
        }
    }
}
