using DNTFrameworkCore.Web.API;
using DNTFrameworkCoreTemplateAPI.Application.Identity;
using DNTFrameworkCoreTemplateAPI.Application.Identity.Models;
using DNTFrameworkCoreTemplateAPI.API.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DNTFrameworkCoreTemplateAPI.API.Controllers
{
    [Route("api/[controller]")]
    public class RolesController : CrudController<IRoleService, long, RoleReadModel, RoleModel>
    {
        public RolesController(IRoleService service) : base(service)
        {
        }

        protected override string CreatePermissionName => PermissionNames.Roles_Create;
        protected override string EditPermissionName => PermissionNames.Roles_Edit;
        protected override string ViewPermissionName => PermissionNames.Roles_View;
        protected override string DeletePermissionName => PermissionNames.Roles_Delete;
    }
}