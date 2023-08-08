using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyPlatform.Data.Models;
using StudyPlatform.Infrastructure;
using StudyPlatform.Services.Users.Interfaces;
using static StudyPlatform.Common.GeneralConstants;

namespace StudyPlatform.Controllers
{
    [Authorize]
    public class TeacherController : Controller
    {
        private readonly ITeacherService _teacherService;
        private readonly ITeacherFormService _teacherFormService;
        private readonly UserManager<ApplicationUser> _userManager;
        public TeacherController(
            ITeacherService teacherService,
            ITeacherFormService teacherFormService,
            UserManager<ApplicationUser> userManager)
        {
            this._teacherService = teacherService;
            this._teacherFormService = teacherFormService;
            this._userManager = userManager;
        }
        /// <summary>
        /// GET method for turning an User into a teacher.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> BecomeTeacher()
        {
            ApplicationUser appUser = await this._userManager.GetUserAsync(User);
            bool isRegularUser = await this._userManager.IsInRoleAsync(appUser, UserRoleName);
            if (!isRegularUser)
            {
                return RedirectToAction("Error", "Home", new { statusCode = 401 });
            }

            return View();
        }

        /// <summary>
        /// POST method for turning an User into a teacher.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> BecomeTeacher(string submit)
        {
            ApplicationUser appUser = await this._userManager.GetUserAsync(User);
            bool isRegularUser = await this._userManager.IsInRoleAsync(appUser, UserRoleName);
            if (!isRegularUser)
            {
                return RedirectToAction("Error", "Home", new { statusCode = 401 });
            }

            if (string.IsNullOrWhiteSpace(submit) || submit.ToLower().Equals("no")) {
                return RedirectToAction("Index", "Home");
            }

            Guid userId = User.Id();
            if (userId == Guid.Empty) {
                return RedirectToAction("Error", "Home", new { StatusCode = 400 });
            }

            try {
                await this._teacherFormService.AddTeacherAsync(userId);

                await this._teacherFormService.UpdateRoleToTeacherAsync(userId, appUser);
            } catch(InvalidOperationException ioe) {
                return RedirectToAction("Error", "Home", new {StatusCode = 500});
            } catch(Exception ex) {
                return RedirectToAction("Error", "Home", new { StatusCode = 500 });
            }
            
            return RedirectToAction("GetProfile", "Account");
        }
    }
}
