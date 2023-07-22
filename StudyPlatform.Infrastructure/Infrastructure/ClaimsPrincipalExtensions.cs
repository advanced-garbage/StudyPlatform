using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using static StudyPlatform.Common.GeneralConstants;

namespace StudyPlatform.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid Id(this ClaimsPrincipal user)
        {
            return new Guid(user.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        public static string Name(this ClaimsPrincipal user)
        {
            return new string(user.FindFirst(ClaimTypes.Name).Value);
        }

        public static bool IsTeacher(this ClaimsPrincipal user)
        {
            return user.Identity.IsAuthenticated && user.IsInRole(TeacherRoleName);
        }

        public static bool IsAdministrator(this ClaimsPrincipal user)
        {
            return user.Identity.IsAuthenticated && user.IsInRole(AdministratorRole);
        }

        public static string GetRole(this ClaimsPrincipal user)
        {
            return user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(x => x.Value).FirstOrDefault();
        }
    }
}
