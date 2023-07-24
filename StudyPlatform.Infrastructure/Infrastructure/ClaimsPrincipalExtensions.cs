using Microsoft.AspNetCore.Identity;
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
            bool isTeacher = false;
            foreach (var identity in user.Identities)
            {
                if (identity.Claims.Where(c => c.Type == ClaimTypes.Role).First().Value == "Teacher")
                {
                    isTeacher = true;
                    break;
                }
            }
            return user.Identity.IsAuthenticated && isTeacher;
        }

        public static bool IsAdministrator(this ClaimsPrincipal user)
        {
            return user.Identity.IsAuthenticated && user.IsInRole(AdministratorRoleName);
        }

        public static string GetRole(this ClaimsPrincipal user)
        {
            return user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(x => x.Value).FirstOrDefault();
        }

        public static void AddTeacherRoleIdentity(this ClaimsPrincipal user)
        {
            var teacherIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, TeacherRoleName)
            });

            user.AddIdentity(teacherIdentity);
        }

        public static void AddAdministratorRoleIdentity(this ClaimsPrincipal user)
        {
            var adminIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, AdministratorRoleName)
            });
            user.AddIdentity(adminIdentity);
        }
    }
}
