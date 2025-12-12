using ByteLabs.FinanceServices.Payables.Localization;
using ByteLabs.Foundations.AspNetCore.Mvc;

namespace ByteLabs.FinanceServices.Payables;

public abstract class PayablesController : AspNetCoreController
{
    protected PayablesController()
    {
        LocalizationResource = typeof(PayablesResource);
    }
}
