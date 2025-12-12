using ByteLabs.FinanceServices.Recevables.Localization;
using ByteLabs.Foundations.Application.Services;

namespace ByteLabs.FinanceServices.Recevables;

public abstract class RecevablesServiceAppService : ApplicationService
{
    protected RecevablesServiceAppService()
    {
        LocalizationResource = typeof(RecevablesResource);
        ObjectMapperContext = typeof(RecevablesApplicationModule);
    }
}
