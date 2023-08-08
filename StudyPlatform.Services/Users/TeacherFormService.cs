using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudyPlatform.Data;
using StudyPlatform.Data.Models;
using StudyPlatform.Services.Users.Interfaces;
using Microsoft.AspNetCore.Http;
using static StudyPlatform.Common.GeneralConstants;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TeacherFormService(
            StudyPlatformDbContext db,
            RoleManager<IdentityRole<Guid>> roleManager,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            this._db = db;
            this._roleManager = roleManager;
            this._userManager = userManager;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task AddTeacherAsync(Guid id)
        {
            Teacher teacherObj= new Teacher()
            {
                Id = id
            };

            await this._db.Teachers.AddAsync(teacherObj);
            await this._db.SaveChangesAsync();
        }

        public async Task UpdateRoleToTeacherAsync(Guid id)
        {
            string teacherRoleName = TeacherRoleName;
            string userRoleName = UserRoleName;
            bool teacherRoleExists = await this._roleManager.RoleExistsAsync(teacherRoleName);
            bool studentRoleExists = await this._roleManager.RoleExistsAsync(userRoleName);
            ApplicationUser appUser = await this._userManager.GetUserAsync(this._httpContextAccessor.HttpContext.User);
            //ClaimsPrincipal claimsUser = this._httpContextAccessor.HttpContext.User;
            if (teacherRoleExists && studentRoleExists)
            {
                IdentityResult addTeacherRoleResult = await this._userManager.AddToRoleAsync(appUser, teacherRoleName);

                if (!addTeacherRoleResult.Succeeded)
                { 
                    throw new InvalidOperationException("Error from adding the teacher role to the user");
                }
            } else
            {
                throw new InvalidOperationException("Something went wrong with adding a role to the user");
            }
        }
    }
}
