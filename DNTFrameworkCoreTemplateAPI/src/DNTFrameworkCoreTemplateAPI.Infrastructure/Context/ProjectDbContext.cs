using System.Linq;
using DNTFrameworkCore.EFCore.Caching;
using DNTFrameworkCore.EFCore.Context;
using DNTFrameworkCore.EFCore.Context.Hooks;
using DNTFrameworkCore.EFCore.Logging;
using DNTFrameworkCore.EFCore.Protection;
using DNTFrameworkCore.EFCore.SqlServer.Numbering;
using DNTFrameworkCore.Runtime;
using DNTFrameworkCoreTemplateAPI.Infrastructure.Mappings.Identity;
using EFSecondLevelCache.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DNTFrameworkCoreTemplateAPI.Infrastructure.Context
{
    public class ProjectDbContext : DbContextCore
    {
        public ProjectDbContext(
            IHookEngine hookEngine,
            IUserSession session,
            DbContextOptions<ProjectDbContext> options) : base(hookEngine, session, options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyLogConfiguration();
            modelBuilder.ApplyNumberedEntityConfiguration();
            modelBuilder.ApplyProtectionKeyConfiguration();
            modelBuilder.ApplySqlCacheConfiguration();

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

        protected override void AfterSaveChanges(EntityChangeContext context)
        {
            this.GetService<IEFCacheServiceProvider>()
                .InvalidateCacheDependencies(context.EntityNames.ToArray());
        }
    }
}