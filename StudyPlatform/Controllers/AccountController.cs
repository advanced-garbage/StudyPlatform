using Microsoft.AspNetCore.Mvc;
using StudyPlatform.Infrastructure;
using StudyPlatform.Services.Users.Interfaces;
using StudyPlatform.Web.View.Models.User;
using static StudyPlatform.Infrastructure.ClaimsPrincipalExtensions;
using static StudyPlatform.Common.ViewModelConstants.Account;
using StudyPlatform.Services.Lesson.Interfaces;
using StudyPlatform.Services.Roles.Interfaces;

namespace StudyPlatform.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILessonViewService _lessonViewService;
        private readonly IRoleService _roleService;

        public AccountController(
            IUserService userService, 
            ILessonViewService lessonViewService,
            IRoleService roleService)
        {
            this._userService = userService;
            this._lessonViewService = lessonViewService;
            this._roleService = roleService;
        }

        /// <summary>
        /// Action which redirects the user to their profile.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Route("account/home")]
        //[Route("account/index")]
        //[Route("account/")]
        public async Task<IActionResult> GetProfile(string username)
        {
            try {
                Guid userId = await this._userService.GetGuidByUsernameAsync(username);
                if (userId == null || userId == Guid.Empty)
                {
                    return RedirectToAction("Index", "Home");
                }

                UserAccountViewModel userModel = await this._userService.GetUserByIdAsync(userId);
                userModel.RoleTitle = await this._roleService.GetRoleNameAsync();
                
                if (await this._roleService.IsTeacherRole())
                {
                    userModel.Lessons = await this._lessonViewService.GetAccountCreditsAsync(userId);
                }

                return View(userModel);
            } catch {
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }
        }
    }
}
