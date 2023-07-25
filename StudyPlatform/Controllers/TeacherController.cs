using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public TeacherController(
            ITeacherService teacherService,
            ITeacherFormService teacherFormService)
        {
            this._teacherService = teacherService;
            this._teacherFormService = teacherFormService;
        }
        /// <summary>
        /// GET method for turning an User into a teacher.
        /// </summary>
        [Authorize(Roles = UserRoleName)]
        [HttpGet]
        public IActionResult BecomeTeacher()
        {
            return View();
        }
        /// <summary>
        /// POST method for turning an User into a teacher.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = UserRoleName)]
        [HttpPost]
        public async Task<IActionResult> BecomeTeacher(string submit)
        {
            if (string.IsNullOrWhiteSpace(submit) || submit.ToLower().Equals("no")) {
                return RedirectToAction("Index", "Home");
            }

            Guid userId = User.Id();
            if (userId == Guid.Empty) {
                return RedirectToAction("Error", "Home", new { StatusCode = 400 });
            }

            try {
                await this._teacherFormService.AddTeacher(userId);
                await this._teacherFormService.UpdateRoleToTeacher(userId);
            } catch(InvalidOperationException ioe) {
                return RedirectToAction("Error", "Home", new {StatusCode = 500});
            } catch(Exception ex) {
                return RedirectToAction("Error", "Home", new { StatusCode = 500 });
            }
            
            return RedirectToAction("GetProfile", "Account");
        }
    }
}
