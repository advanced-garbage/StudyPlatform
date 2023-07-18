using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudyPlatform.Data;
using StudyPlatform.Data.Models;
using StudyPlatform.Services.Users.Interfaces;
using static StudyPlatform.Common.ViewModelConstants.Account;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace StudyPlatform.Services.Users
{
    public class TeacherFormService : ITeacherFormService
    {
        /// <summary>
        /// DbContext dependency.
        /// </summary>
        private readonly StudyPlatformDbContext _db;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TeacherFormService(
            StudyPlatformDbContext db,
            RoleManager<IdentityRole<Guid>> roleManager,
            IConfiguration config,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            this._db = db;
            this._roleManager = roleManager;
            this._config = config;
            this._userManager = userManager;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task AddTeacher(Guid id)
        {
            Teacher teacherObj= new Teacher()
                {
                    Id = id
                };

            await this._db.Teachers.AddAsync(teacherObj);
            await this._db.SaveChangesAsync();
        }

        public async Task UpdateRoleToTeacher(Guid id)
        {
            string roleName = this._config["RoleNames:TeacherRoleName"];
            bool roleExists = await this._roleManager.RoleExistsAsync(roleName);

            if (roleExists)
            {
                ApplicationUser user = await this._userManager.GetUserAsync(this._httpContextAccessor.HttpContext.User);
                IdentityResult result = await this._userManager.AddToRoleAsync(user, roleName);

                // Something went wrong
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("Something went wrong with adding a role to the user");
                }
            } else
            {
                throw new InvalidOperationException("Something went wrong with adding a role to the user");
            }
        }
    }
}
