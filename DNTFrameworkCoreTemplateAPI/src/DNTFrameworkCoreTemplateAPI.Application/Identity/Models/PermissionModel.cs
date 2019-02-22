using DNTFrameworkCore.Application.Models;

namespace DNTFrameworkCoreTemplateAPI.Application.Identity.Models
{
    public class PermissionModel : DetailModel<int>
    {
        public string Name { get; set; }
    }
}