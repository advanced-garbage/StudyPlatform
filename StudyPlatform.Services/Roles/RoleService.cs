using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StudyPlatform.Data.Models;
using StudyPlatform.Services.Roles.Interfaces;
using System.Security.Claims;
using static StudyPlatform.Common.GeneralConstants;
using static StudyPlatform.Infrastructure.ClaimsPrincipalExtensions;

namespace StudyPlatform.Services.Roles
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static IEnumerable<string> _roles;

        public RoleService(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            this._userManager = userManager;
            this._httpContextAccessor = httpContextAccessor;
            _roles = new List<string>() { AdministratorRoleName, TeacherRoleName };
        }

        public async Task<string> GetRoleNameAsync()
        {
            ApplicationUser user = await this._userManager.GetUserAsync(this._httpContextAccessor.HttpContext.User);

            foreach (string role in _roles)
            {
                if (await this._userManager.IsInRoleAsync(user, role))
                {
                    return role;
                }
            }

            return StudentRoleName;
        }

        public async Task<bool> IsTeacherRole()
        {
            ApplicationUser user = await this._userManager.GetUserAsync(this._httpContextAccessor.HttpContext.User);
            bool isTeacher = await this._userManager.IsInRoleAsync(user, TeacherRoleName);

            return isTeacher;
        }
    }
}
