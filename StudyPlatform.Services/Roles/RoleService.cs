using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StudyPlatform.Data.Models;
using StudyPlatform.Services.Roles.Interfaces;
using System.Security.Claims;

namespace StudyPlatform.Services.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleService(
            RoleManager<IdentityRole<Guid>> roleManager,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
            this._httpContextAccessor = httpContextAccessor;
        }

        public string GetRoleName()
        {
            string roleName = this._httpContextAccessor
                              .HttpContext
                              .User
                              .FindAll(ClaimTypes.Role)
                              .First()
                              .Value;
            return roleName;
        }
    }
}
