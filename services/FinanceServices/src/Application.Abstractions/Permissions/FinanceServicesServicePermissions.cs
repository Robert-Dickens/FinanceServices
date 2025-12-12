using ByteLabs.Foundations.Reflection;

namespace ByteLabs.FinanceServices.Services.FinanceServices.Permissions;

public class FinanceServicesServicePermissions
{
    public const string GroupName = FinanceServicesServiceConsts.ModuleName;

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(FinanceServicesServicePermissions));
    }

    public class Products
    {
        public const string Default = GroupName + ".Products";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
}
