using ByteLabs.FinanceServices;
using MyCompanyName.FinanceServices;

const string AppPrefix = "FinanceServices";

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddForwardedHeaders();

const string LaunchEntry = "Aspire";

IResourceBuilder<RedisResource> redis = builder.AddRedis(AppPrefix + "Redis", 50939)
    .WithRedisInsight()
    .WithRedisCommander()
    .WithLifetime(ContainerLifetime.Persistent); IResourceBuilder<RabbitMQServerResource> rabbitMq = builder.AddRabbitMQ(AppPrefix + "RabbitMq").WithLifetime(ContainerLifetime.Persistent);

/*
 * 
 * if you want to use a known password for the SQL Server, you can set it in the user secrets
 * //dotnet user-secrets set Parameters:sql-password jkhldfsghiujosd879@@o45t3hkj
var sqlPassword = builder.AddParameter("sql-password", secret: true);

var sql = builder.AddSqlServer(AppPrefix + "Sql", password: sqlPassword);
 * 
 * 
 * If you want to persist the databases you can enable the docker volumes
 * .WithDataVolume()
 * 
 */
IResourceBuilder<PostgresServerResource> sql = builder.AddPostgres(AppPrefix + "PgSql")
                .WithImage("ankane/pgvector")
                .WithImageTag("latest").WithDataVolume().WithLifetime(ContainerLifetime.Persistent)
                .WithPgAdmin(c => c.WithHostPort(8999)
                .WithImageTag("latest")).WithLifetime(ContainerLifetime.Persistent);

IResourceBuilder<PostgresDatabaseResource> FinanceServicesServiceDb = sql.AddDatabase("FinanceServicesServiceDB");
IResourceBuilder<PostgresDatabaseResource> FinanceServicesAppDb = sql.AddDatabase("FinanceServicesAppDB");

IResourceBuilder<ProjectResource> dbMigrator = builder.AddProject<Projects.DbMigrator>("DbMigrator").ExcludeFromManifest()
                            .WithReference(FinanceServicesServiceDb, GlobalConstants.Databases.AdministrationServiceConnectionStringName)
                            .WithReference(FinanceServicesServiceDb, GlobalConstants.Databases.SaasServiceConnectionStringName)
                            .WithReference(FinanceServicesServiceDb, GlobalConstants.Databases.IdentityServiceConnectionStringName)
                            .WithReference(FinanceServicesAppDb, GlobalConstants.Databases.FinanceServicesConnectionStringName)
                            .WaitFor(FinanceServicesServiceDb);

IResourceBuilder<ProjectResource> identityServer = builder.AddProject<Projects.AuthServer_Web>(GlobalConstants.Services.IdentityStsServiceName, LaunchEntry)
                                           .WithReference(FinanceServicesServiceDb, GlobalConstants.Databases.IdentityServiceConnectionStringName).WaitFor(FinanceServicesServiceDb)
                                           .WithReference(FinanceServicesServiceDb, GlobalConstants.Databases.AdministrationServiceConnectionStringName)
                                           .WithReference(FinanceServicesServiceDb, GlobalConstants.Databases.SaasServiceConnectionStringName)
                                           .WithReference(redis, GlobalConstants.Services.RedisConnectionStringName).WaitFor(redis)
                                           .WithReference(rabbitMq, GlobalConstants.Services.RabbitMqStringName).WaitFor(rabbitMq)
                                           .WaitForCompletion(dbMigrator)
                                           .WithExternalHttpEndpoints().ConfigureSelf();

identityServer.WithEnvironment(context =>
{
    EndpointReference selfUrl = identityServer.GetEndpoint("https");
    context.EnvironmentVariables["App__TokenIssuerUrl"] = selfUrl;
});

IResourceBuilder<ProjectResource> administrationServiceApiHost = builder.AddProject<Projects.AdministrationService_HttpApi_Host>(GlobalConstants.Services.AdministrationServiceName, LaunchEntry)
                                           .WithReference(FinanceServicesServiceDb, GlobalConstants.Databases.AdministrationServiceConnectionStringName).WaitFor(FinanceServicesServiceDb)
                                           .WithReference(FinanceServicesServiceDb, GlobalConstants.Databases.SaasServiceConnectionStringName)
                                           .WithReference(redis, GlobalConstants.Services.RedisConnectionStringName).WaitFor(redis)
                                           .WithReference(rabbitMq, GlobalConstants.Services.RabbitMqStringName).WaitFor(rabbitMq)
                                           .WithReference(identityServer).WaitFor(identityServer)
                                           .WithExternalHttpEndpoints().ConfigureSelf();


IResourceBuilder<ProjectResource> identityServiceApiHost = builder.AddProject<Projects.IdentityService_HttpApi_Host>(GlobalConstants.Services.IdentityServiceName, LaunchEntry)
                                           .WithReference(FinanceServicesServiceDb, GlobalConstants.Databases.IdentityServiceConnectionStringName).WaitFor(FinanceServicesServiceDb)
                                           .WithReference(FinanceServicesServiceDb, GlobalConstants.Databases.SaasServiceConnectionStringName)
                                           .WithReference(FinanceServicesServiceDb, GlobalConstants.Databases.AdministrationServiceConnectionStringName)
                                           .WithReference(redis, GlobalConstants.Services.RedisConnectionStringName).WaitFor(redis)
                                           .WithReference(rabbitMq, GlobalConstants.Services.RabbitMqStringName).WaitFor(rabbitMq)
                                           .WithReference(identityServer).WaitFor(identityServer)
                                           .WithExternalHttpEndpoints().ConfigureSelf();

IResourceBuilder<ProjectResource> saasServiceApiHost = builder.AddProject<Projects.SaasService_HttpApi_Host>(GlobalConstants.Services.SaasServiceName, LaunchEntry)
                                           .WithReference(FinanceServicesServiceDb, GlobalConstants.Databases.SaasServiceConnectionStringName).WaitFor(FinanceServicesServiceDb)
                                           .WithReference(FinanceServicesServiceDb, GlobalConstants.Databases.AdministrationServiceConnectionStringName)
                                           .WithReference(redis, GlobalConstants.Services.RedisConnectionStringName).WaitFor(redis)
                                           .WithReference(rabbitMq, GlobalConstants.Services.RabbitMqStringName).WaitFor(rabbitMq)
                                           .WithReference(identityServer).WaitFor(identityServer)
                                           .WithExternalHttpEndpoints().ConfigureSelf();

IResourceBuilder<ProjectResource> myProjectNameServiceApiHost = builder.AddProject<Projects.FinanceServicesService_HttpApi_Host>(GlobalConstants.Services.FinanceServicesServiceName, LaunchEntry)
                                           .WithReference(FinanceServicesAppDb, GlobalConstants.Databases.FinanceServicesConnectionStringName).WaitFor(FinanceServicesAppDb)
                                           .WithReference(FinanceServicesServiceDb, GlobalConstants.Databases.AdministrationServiceConnectionStringName).WaitFor(FinanceServicesServiceDb)
                                           .WithReference(FinanceServicesServiceDb, GlobalConstants.Databases.SaasServiceConnectionStringName)
                                           .WithReference(redis, GlobalConstants.Services.RedisConnectionStringName).WaitFor(redis)
                                           .WithReference(rabbitMq, GlobalConstants.Services.RabbitMqStringName).WaitFor(rabbitMq)
                                           .WithReference(identityServer).WaitFor(identityServer)
                                           .WithExternalHttpEndpoints().ConfigureSelf();

IResourceBuilder<ProjectResource> webAppGateway = builder.AddProject<Projects.WebGateway_Yarp>(GlobalConstants.Services.GatewayServiceName, LaunchEntry)
                                           .WithReference(identityServer).WaitFor(identityServer)
                                           .WithReference(administrationServiceApiHost).WaitFor(administrationServiceApiHost)
                                           .WithReference(identityServiceApiHost).WaitFor(identityServiceApiHost)
                                           .WithReference(saasServiceApiHost).WaitFor(saasServiceApiHost)
                                           .WithReference(myProjectNameServiceApiHost).WaitFor(myProjectNameServiceApiHost)
                                           .WithReference(redis, GlobalConstants.Services.RedisConnectionStringName).WaitFor(redis)
                                           .WithExternalHttpEndpoints().ConfigureSelf();

builder.AddProject<Projects.ManagementPortal_Web>(GlobalConstants.Clients.ManagementPortalServiceName, LaunchEntry)
                            .WithReference(identityServer).WaitFor(identityServer)
                            .WithReference(webAppGateway).WaitFor(webAppGateway)
                            .WithReference(redis, GlobalConstants.Services.RedisConnectionStringName).WaitFor(redis)
                            .WithExternalHttpEndpoints().ConfigureSelf();

builder.AddProject<Projects.ManagementPortal_Blazor>(GlobalConstants.Clients.BlazorManagementPortalServiceName, LaunchEntry)
                            .WithReference(identityServer).WaitFor(identityServer)
                            .WithReference(webAppGateway).WaitFor(webAppGateway)
                            .WithReference(redis, GlobalConstants.Services.RedisConnectionStringName).WaitFor(redis)
                            .WithExternalHttpEndpoints().ConfigureSelf();


builder.AddProject<Projects.Public_Web>(GlobalConstants.Clients.PublicPortalServiceName, LaunchEntry)
                            .WithReference(identityServer).WaitFor(identityServer)
                            .WithReference(webAppGateway).WaitFor(webAppGateway)
                            .WithReference(redis, GlobalConstants.Services.RedisConnectionStringName).WaitFor(redis)
                            .WithExternalHttpEndpoints().ConfigureSelf();

builder.Build().Run();
