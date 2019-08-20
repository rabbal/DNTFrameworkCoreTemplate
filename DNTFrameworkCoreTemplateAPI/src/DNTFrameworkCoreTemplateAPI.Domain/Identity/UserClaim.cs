using System.Security.Claims;
using DNTFrameworkCore.Domain;

namespace DNTFrameworkCoreTemplateAPI.Domain.Identity
{
    public class UserClaim : TrackableEntity, ICreationTracking
    {
        public const int MaxClaimTypeLength = 256;

        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public User User { get; set; }
        public long UserId { get; set; }
    }
}