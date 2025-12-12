using ByteLabs.Foundations.Data;
using ByteLabs.Foundations.DependencyInjection;
using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Platform.Foundations.Domain.Context;
using ByteLabs.PlatformServices.AuditLogging.Domain;
using ByteLabs.PlatformServices.AuditLogging.Domain.Context;
using ByteLabs.PlatformServices.Features.Domain;
using ByteLabs.PlatformServices.Features.Domain.Context;
using ByteLabs.PlatformServices.Language;
using ByteLabs.PlatformServices.Language.EntityFrameworkCore;
using ByteLabs.PlatformServices.Language.External;
using ByteLabs.PlatformServices.Permissions.Domain;
using ByteLabs.PlatformServices.Permissions.Domain.Context;
using ByteLabs.PlatformServices.Settings.Domain;
using ByteLabs.PlatformServices.Settings.Domain.Context;
using Microsoft.EntityFrameworkCore;

namespace ByteLabs.FinanceServices.Services.Administration.Domain.Context;

[ConnectionStringName(AdministrationServiceDbProperties.ConnectionStringName)]
[ReplaceDbContext(typeof(IPlatformServicesDbContext), typeof(IAuditLoggingDbContext), typeof(IFeatureManagementDbContext), typeof(ILanguageManagementDbContext), typeof(IPermissionManagementDbContext), typeof(ISettingManagementDbContext))]
public class AdministrationServiceDbContext : PlatformDbContext<AdministrationServiceDbContext>, IAdministrationServiceDbContext
{

    public AdministrationServiceDbContext(DbContextOptions<AdministrationServiceDbContext> options)
        : base(options)
    {
    }

    public DbSet<AuditLog> AuditLogs { get; protected internal set; }

    public DbSet<FeatureGroupDefinitionRecord> FeatureGroups { get; protected internal set; }

    public DbSet<FeatureDefinitionRecord> Features { get; protected internal set; }

    public DbSet<FeatureValue> FeatureValues { get; protected internal set; }

    public DbSet<Language> Languages { get; protected internal set; }

    public DbSet<LanguageText> LanguageTexts { get; protected internal set; }

    public DbSet<LocalizationResourceRecord> LocalizationResources { get; protected internal set; }

    public DbSet<LocalizationTextRecord> LocalizationTexts { get; protected internal set; }

    public DbSet<PermissionGroupDefinitionRecord> PermissionGroups { get; protected internal set; }

    public DbSet<PermissionDefinitionRecord> Permissions { get; protected internal set; }

    public DbSet<PermissionGrant> PermissionGrants { get; protected internal set; }

    public DbSet<Setting> Settings { get; protected internal set; }

    public DbSet<SettingDefinitionRecord> SettingDefinitionRecords { get; protected internal set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureAuditLogging();
        modelBuilder.ConfigureFeatureManagement();
        modelBuilder.ConfigureLanguageManagement();
        modelBuilder.ConfigurePermissionManagement();
        modelBuilder.ConfigureSettingManagement();
    }
}
