using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.PlatformServices.Identity.Domain;
using ByteLabs.PlatformServices.Identity.Domain.Claims;
using ByteLabs.PlatformServices.Identity.Domain.Context;
using ByteLabs.PlatformServices.Identity.Domain.Linking;
using ByteLabs.PlatformServices.Identity.Domain.Organizations;
using ByteLabs.PlatformServices.Identity.Domain.Roles;
using ByteLabs.PlatformServices.Identity.Domain.Security;
using ByteLabs.PlatformServices.Identity.Domain.Users;
using ByteLabs.PlatformServices.Security.IdentityServer.Domain.ApiResources;
using ByteLabs.PlatformServices.Security.IdentityServer.Domain.ApiScopes;
using ByteLabs.PlatformServices.Security.IdentityServer.Domain.Clients;
using ByteLabs.PlatformServices.Security.IdentityServer.Domain.Context;
using ByteLabs.PlatformServices.Security.IdentityServer.Domain.Devices;
using ByteLabs.PlatformServices.Security.IdentityServer.Domain.Grants;
using ByteLabs.PlatformServices.Security.IdentityServer.Domain.IdentityResources;
using Microsoft.EntityFrameworkCore;

namespace ByteLabs.FinanceServices.Services.Identity.Domain.Context;

[ConnectionStringName(IdentityServiceDbProperties.ConnectionStringName)]
public class IdentityServiceDbContext : PlatformDbContext<IdentityServiceDbContext>, IIdentityServiceDbContext, IIdentityDbContext, IIdentityServerDbContext
{


    public IdentityServiceDbContext(DbContextOptions<IdentityServiceDbContext> options)
        : base(options)
    {

    }

    public DbSet<IdentityPersona> Users { get; protected set; }

    public DbSet<IdentityRole> Roles { get; protected set; }

    public DbSet<IdentityClaimType> ClaimTypes { get; protected set; }

    public DbSet<OrganizationUnit> OrganizationUnits { get; protected set; }

    public DbSet<IdentitySecurityLog> SecurityLogs { get; protected set; }

    public DbSet<IdentityLinkUser> LinkUsers { get; protected set; }

    public DbSet<IdentityUserDelegation> UserDelegations { get; protected set; }

    public DbSet<IdentitySession> Sessions { get; protected set; }

    public DbSet<ApiResource> ApiResources { get; protected set; }

    public DbSet<ApiResourceSecret> ApiResourceSecrets { get; protected set; }

    public DbSet<ApiResourceClaim> ApiResourceClaims { get; protected set; }

    public DbSet<ApiResourceScope> ApiResourceScopes { get; protected set; }

    public DbSet<ApiResourceProperty> ApiResourceProperties { get; protected set; }

    public DbSet<ApiScope> ApiScopes { get; protected set; }

    public DbSet<ApiScopeClaim> ApiScopeClaims { get; protected set; }

    public DbSet<ApiScopeProperty> ApiScopeProperties { get; protected set; }

    public DbSet<IdentityResource> IdentityResources { get; protected set; }

    public DbSet<IdentityResourceClaim> IdentityClaims { get; protected set; }

    public DbSet<IdentityResourceProperty> IdentityResourceProperties { get; protected set; }

    public DbSet<Client> Clients { get; protected set; }

    public DbSet<ClientGrantType> ClientGrantTypes { get; protected set; }

    public DbSet<ClientRedirectUri> ClientRedirectUris { get; protected set; }

    public DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; protected set; }

    public DbSet<ClientScope> ClientScopes { get; protected set; }

    public DbSet<ClientSecret> ClientSecrets { get; protected set; }

    public DbSet<ClientClaim> ClientClaims { get; protected set; }

    public DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; protected set; }

    public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; protected set; }

    public DbSet<ClientProperty> ClientProperties { get; protected set; }

    public DbSet<PersistedGrant> PersistedGrants { get; protected set; }

    public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; protected set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureIdentity();

        modelBuilder.ConfigureIdentityServer();
    }
}
