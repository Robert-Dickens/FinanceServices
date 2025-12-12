using ByteLabs.FinanceServices.Services.FinanceServices.Localization;
using ByteLabs.Foundations.AspNetCore.Mvc;

namespace ByteLabs.FinanceServices.Services.FinanceServices;

public abstract class FinanceServicesServiceController : AspNetCoreController
{
    protected FinanceServicesServiceController()
    {
        LocalizationResource = typeof(FinanceServicesServiceResource);
    }
}
