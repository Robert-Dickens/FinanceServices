namespace ByteLabs.FinanceServices
{
    public class GlobalConstants
    {
        public class Identity
        {
            public const string DefaultAdminEmailAddress = "admin@admin.com";

            public const string DefaultAdminPassword = "Admin!P@ssword592";
        }

        public class Databases
        {
            public const string FinanceServicesConnectionStringName = "FinanceServicesService";
            public const string IdentityServiceConnectionStringName = "IdentityService";
            public const string SaasServiceConnectionStringName = "SaasService";
            public const string AdministrationServiceConnectionStringName = "AdministrationService";
        }

        public class Services
        {
            public const string RedisConnectionStringName = "Redis";
            public const string RabbitMqStringName = "RabbitMq";

            public const string IdentityStsServiceName = "IdentitySts";
            public const string AdministrationServiceName = "AdministrationService";
            public const string IdentityServiceName = "IdentityService";
            public const string SaasServiceName = "SaasService";
            public const string FinanceServicesServiceName = "FinanceServicesService";

            public const string GatewayServiceName = "GatewayService";
            public const string PublicGatewayServiceName = "PublicGatewayService";

            public const string AccountServiceName = "AccountService";

        }

        public class Modules
        {
            public const string FinanceServices = "FinanceServices";
        }

        public class Clients
        {
            public const string BlazorManagementPortalServiceName = "BlazorManagementPortal";
            public const string ManagementPortalServiceName = "ManagementPortal";
            public const string PublicPortalServiceName = "PublicPortal";
        }
    }
}
