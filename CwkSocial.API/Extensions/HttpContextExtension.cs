using System.Security.Claims;

namespace CwkSocial.API.Extensions
{
    public static class HttpContextExtension
    {
        public static Guid GetUserProfileIdClaimValue(this HttpContext context)
        {
            return GetGuidIdClaimValue("UserProfileId", context);
        }

        public static Guid GetIdentityIdClaimValue(this HttpContext context)
        {
            return GetGuidIdClaimValue("IdentityId", context);
        }

        private static Guid GetGuidIdClaimValue(string key, HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;

            return Guid.Parse(identity?.FindFirst($"{key}")?.Value);
        }
    }
}