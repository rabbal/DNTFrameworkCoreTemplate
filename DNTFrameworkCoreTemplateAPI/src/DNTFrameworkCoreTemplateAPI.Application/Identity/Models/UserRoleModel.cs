using DNTFrameworkCore.Application.Models;

namespace DNTFrameworkCoreTemplateAPI.Application.Identity.Models
{
    public class UserRoleModel : DetailModel<int>
    {
        public long RoleId { get; set; }
    }
}