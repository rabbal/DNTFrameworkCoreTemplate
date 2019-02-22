using System.Linq;
using DNTFrameworkCore.EntityFramework.Auditing;
using DNTFrameworkCore.EntityFramework.Caching;
using DNTFrameworkCore.EntityFramework.Context;
using DNTFrameworkCore.EntityFramework.DataProtection;
using DNTFrameworkCore.EntityFramework.Logging;
using DNTFrameworkCoreTemplateAPI.Infrastructure.Mappings.Identity;
using EFSecondLevelCache.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DNTFrameworkCoreTemplateAPI.Infrastructure.Context
{
    public class ProjectDbContext : DbContextCore
    {
        public ProjectDbContext(DbContextCoreDependency<ProjectDbContext> dependency) : base(dependency)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyLogConfiguration();
            modelBuilder.ApplyDataProtectionKeyConfiguration();
            modelBuilder.ApplySqlCacheConfiguration();
            modelBuilder.ApplyAuditLogConfiguration();

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserTokenConfiguration());
            modelBuilder.ApplyConfiguration(new UserPermissionConfiguration());
            modelBuilder.ApplyConfiguration(new UserClaimConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new RoleClaimConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        protected override void AfterSaveChanges(SaveChangeContext context)
        {
            this.GetService<IEFCacheServiceProvider>()
                .InvalidateCacheDependencies(context.ChangedEntityNames.ToArray());
        }
    }
}