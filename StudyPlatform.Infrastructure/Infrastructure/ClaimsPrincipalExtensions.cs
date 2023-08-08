using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using static StudyPlatform.Common.GeneralConstants;

namespace StudyPlatform.Infrastructure
{
    /// <summary>
    /// Class for holding extension methods for Claims Principal.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Returns the Guid of the ClaimsPrincipal User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Guid Id(this ClaimsPrincipal user)
        {
            return new Guid(user.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        /// <summary>
        /// Returns the UserName of the ClaimsPrincipal User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string Name(this ClaimsPrincipal user)
        {
            return new string(user.FindFirst(ClaimTypes.Name).Value);
        }

        /// <summary>
        /// Answers wether or not the User is of role "Teacher".
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Answers wether or not the User is of role "Administrator".
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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
