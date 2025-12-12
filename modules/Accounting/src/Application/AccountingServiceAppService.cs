using ByteLabs.Foundations.Application.Services;
using ByteLabs.FinanceServices.Accounting.Localization;

namespace ByteLabs.FinanceServices.Accounting;

public abstract class AccountingServiceAppService : ApplicationService
{
    protected AccountingServiceAppService()
    {
        LocalizationResource = typeof(AccountingResource);
        ObjectMapperContext = typeof(AccountingApplicationModule);
    }
}
