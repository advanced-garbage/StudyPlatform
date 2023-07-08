using System.Security.Claims;

namespace StudyPlatform.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid Id(this ClaimsPrincipal user)
        {
            return new Guid(user.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}
