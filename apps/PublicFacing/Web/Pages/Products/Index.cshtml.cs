using ByteLabs.Foundations.AspNetCore.Mvc.UI.RazorPages;

namespace FinanceServices.PublicServer.Web.Pages.Products;

public class Index : AspNetCorePageModel
{
    public Task OnGet()
    {
        return Task.CompletedTask;
    }
}
