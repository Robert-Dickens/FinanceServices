using ByteLabs.FinanceServices.Services.FinanceServices.Localization;
using ByteLabs.Foundations.Application.Services;

namespace ByteLabs.FinanceServices.Services.FinanceServices;

public abstract class FinanceServicesServiceAppService : ApplicationService
{
    protected FinanceServicesServiceAppService()
    {
        LocalizationResource = typeof(FinanceServicesServiceResource);
        ObjectMapperContext = typeof(FinanceServicesServiceApplicationModule);
    }
}
