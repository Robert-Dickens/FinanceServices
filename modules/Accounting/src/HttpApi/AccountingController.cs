using ByteLabs.Foundations.AspNetCore.Mvc;
using ByteLabs.FinanceServices.Accounting.Localization;

namespace ByteLabs.FinanceServices.Accounting;

public abstract class AccountingController : AspNetCoreController
{
    protected AccountingController()
    {
        LocalizationResource = typeof(AccountingResource);
    }
}
