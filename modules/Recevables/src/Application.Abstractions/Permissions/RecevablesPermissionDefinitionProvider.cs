using ByteLabs.FinanceServices.Recevables.Localization;
using ByteLabs.Foundations.Authorization.Permissions;
using ByteLabs.Foundations.Localization;

namespace ByteLabs.FinanceServices.Recevables.Permissions;

public class RecevablesPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(RecevablesPermissions.GroupName, L($"Permission:{RecevablesPermissions.GroupName}"));

        var productPermission = myGroup.AddPermission(RecevablesPermissions.Products.Default, L("Permission:Products"));
        productPermission.AddChild(RecevablesPermissions.Products.Create, L("Permission:Create"));
        productPermission.AddChild(RecevablesPermissions.Products.Edit, L("Permission:Edit"));
        productPermission.AddChild(RecevablesPermissions.Products.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<RecevablesResource>(name);
    }
}
