using ByteLabs.FinanceServices.Payables.Localization;
using ByteLabs.Foundations.Authorization.Permissions;
using ByteLabs.Foundations.Localization;

namespace ByteLabs.FinanceServices.Payables.Permissions;

public class PayablesPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(PayablesPermissions.GroupName, L($"Permission:{PayablesPermissions.GroupName}"));

        var productPermission = myGroup.AddPermission(PayablesPermissions.Products.Default, L("Permission:Products"));
        productPermission.AddChild(PayablesPermissions.Products.Create, L("Permission:Create"));
        productPermission.AddChild(PayablesPermissions.Products.Edit, L("Permission:Edit"));
        productPermission.AddChild(PayablesPermissions.Products.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<PayablesResource>(name);
    }
}
