using ByteLabs.Foundations.Reflection;

namespace ByteLabs.FinanceServices.Payables.Permissions;

public class PayablesPermissions
{
    public const string GroupName = PayablesConsts.ModuleName;
    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(PayablesPermissions));
    }

    public class Products
    {
        public const string Default = GroupName + ".Products";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
}
