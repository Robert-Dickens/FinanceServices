using ByteLabs.Foundations.AspNetCore.Mvc.UI.RazorPages;
using Microsoft.AspNetCore.Authentication;

namespace FinanceServices.PublicServer.Web.Pages;

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
