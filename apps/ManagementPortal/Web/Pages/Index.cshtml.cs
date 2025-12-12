using Microsoft.AspNetCore.Authentication;
using ByteLabs.Foundations.AspNetCore.Mvc.UI.RazorPages;

namespace FinanceServices.ManagementPortal.Web.Pages;

public class IndexModel : AspNetCorePageModel
{
    public void OnGet()
    {

    }

    public async Task OnPostLoginAsync()
    {
        await HttpContext.ChallengeAsync("oidc");
    }
}
