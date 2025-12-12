using ByteLabs.FinanceServices.Payables.Localization;
using ByteLabs.Foundations.Application.Services;

namespace ByteLabs.FinanceServices.Payables;

public abstract class PayablesServiceAppService : ApplicationService
{
    protected PayablesServiceAppService()
    {
        LocalizationResource = typeof(PayablesResource);
        ObjectMapperContext = typeof(PayablesApplicationModule);
    }
}
