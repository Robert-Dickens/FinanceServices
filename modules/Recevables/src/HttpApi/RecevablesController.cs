using ByteLabs.FinanceServices.Recevables.Localization;
using ByteLabs.Foundations.AspNetCore.Mvc;

namespace ByteLabs.FinanceServices.Recevables;

public abstract class RecevablesController : AspNetCoreController
{
    protected RecevablesController()
    {
        LocalizationResource = typeof(RecevablesResource);
    }
}
