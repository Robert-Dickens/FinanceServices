using ByteLabs.Foundations.Reflection;

namespace ByteLabs.FinanceServices.Accounting.Permissions;

public class AccountingPermissions
{
    public const string GroupName = AccountingConsts.ModuleName;
    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(AccountingPermissions));
    }

    public class Products
    {
        public const string Default = GroupName + ".Products";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
}
