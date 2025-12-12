using ByteLabs.Foundations.Authorization.Permissions;
using ByteLabs.Foundations.Localization;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Permissions;

public class FinanceServicesServicePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {

        var myGroup = context.AddGroup(FinanceServicesServicePermissions.GroupName, L($"Permission:{FinanceServicesServicePermissions.GroupName}"));

        var productPermission = myGroup.AddPermission(FinanceServicesServicePermissions.Products.Default, L("Permission:Products"));
        productPermission.AddChild(FinanceServicesServicePermissions.Products.Create, L("Permission:Create"));
        productPermission.AddChild(FinanceServicesServicePermissions.Products.Edit, L("Permission:Edit"));
        productPermission.AddChild(FinanceServicesServicePermissions.Products.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<FinanceServicesServiceResource>(name);
    }
}
