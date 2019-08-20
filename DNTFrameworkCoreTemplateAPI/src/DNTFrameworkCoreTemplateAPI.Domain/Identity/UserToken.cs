using System;
using DNTFrameworkCore.Domain;

namespace DNTFrameworkCoreTemplateAPI.Domain.Identity
{
    public class UserToken : TrackableEntity
    {
        public const int MaxTokenHashLength = 256;

        public string TokenHash { get; set; }
        public DateTimeOffset TokenExpirationDateTime { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
    }
}