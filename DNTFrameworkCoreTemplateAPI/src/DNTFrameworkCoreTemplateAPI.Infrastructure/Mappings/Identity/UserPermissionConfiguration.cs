using DNTFrameworkCoreTemplateAPI.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DNTFrameworkCoreTemplateAPI.Infrastructure.Mappings.Identity
{
    public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> builder)
        {           
            builder.ToTable(nameof(Permission));
        }
    }
}