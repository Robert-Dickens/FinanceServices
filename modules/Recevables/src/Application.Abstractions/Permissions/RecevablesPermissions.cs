using ByteLabs.Foundations.Reflection;

namespace ByteLabs.FinanceServices.Recevables.Permissions;

public class RecevablesPermissions
{
    public const string GroupName = RecevablesConsts.ModuleName;
    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(RecevablesPermissions));
    }

    public class Products
    {
        public const string Default = GroupName + ".Products";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
}
