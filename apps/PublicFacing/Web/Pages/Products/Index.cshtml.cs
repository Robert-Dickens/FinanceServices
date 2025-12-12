using ByteLabs.Foundations.AspNetCore.Mvc.UI.RazorPages;
using FinanceServices.Services.FinanceServicesService.Products;

namespace FinanceServices.PublicServer.Web.Pages.Products;

public class Index : AspNetCorePageModel
{
    private readonly IProductPublicAppService _productPublicAppService;

    public Index(IProductPublicAppService productPublicAppService)
    {
        _productPublicAppService = productPublicAppService;
    }

    public List<ProductDto> Products { get; set; }

    public async Task OnGet()
    {
        Products = await _productPublicAppService.GetListAsync();
    }
}
