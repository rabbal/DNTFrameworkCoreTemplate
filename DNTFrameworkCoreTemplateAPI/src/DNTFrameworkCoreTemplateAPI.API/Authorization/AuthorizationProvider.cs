using System.Collections.Generic;
using DNTFrameworkCore.Authorization;
using DNTFrameworkCore.Localization;

namespace DNTFrameworkCoreTemplateAPI.API.Authorization
{
    public class AuthorizationProvider : IAuthorizationProvider
    {
        public IEnumerable<Permission> ProvidePermissions()
        {
            yield return Permission.CreatePermission(PermissionNames.Roles_View,
                L(PermissionNames.Roles_View));
            yield return Permission.CreatePermission(PermissionNames.Roles_Create,
                L(PermissionNames.Roles_Create));
            yield return Permission.CreatePermission(PermissionNames.Roles_Edit,
                L(PermissionNames.Roles_Edit));
            yield return Permission.CreatePermission(PermissionNames.Roles_Delete,
                L(PermissionNames.Roles_Delete));
            
            yield return Permission.CreatePermission(PermissionNames.Users_View,
                L(PermissionNames.Users_View));
            yield return Permission.CreatePermission(PermissionNames.Users_Create,
                L(PermissionNames.Users_Create));
            yield return Permission.CreatePermission(PermissionNames.Users_Edit,
                L(PermissionNames.Users_Edit));
            yield return Permission.CreatePermission(PermissionNames.Users_Delete,
                L(PermissionNames.Users_Delete));
        }

        private ILocalizableString L(string name)
        {
            return new LocalizableString(name, "PermissionsResource") {ResourceLocation = "Resources"};
        }
    }
}