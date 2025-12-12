using ByteLabs.Foundations.Authorization.Permissions;
using ByteLabs.Foundations.Localization;
using ByteLabs.FinanceServices.Accounting.Localization;

namespace ByteLabs.FinanceServices.Accounting.Permissions;

public class AccountingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AccountingPermissions.GroupName, L($"Permission:{AccountingPermissions.GroupName}"));

        var productPermission = myGroup.AddPermission(AccountingPermissions.Products.Default, L("Permission:Products"));
        productPermission.AddChild(AccountingPermissions.Products.Create, L("Permission:Create"));
        productPermission.AddChild(AccountingPermissions.Products.Edit, L("Permission:Edit"));
        productPermission.AddChild(AccountingPermissions.Products.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AccountingResource>(name);
    }
}
