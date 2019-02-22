using DNTFrameworkCoreTemplateAPI.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNTFrameworkCoreTemplateAPI.Infrastructure.Mappings.Identity
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable(nameof(Permission));
        }
    }
}