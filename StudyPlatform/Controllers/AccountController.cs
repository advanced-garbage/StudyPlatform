using Microsoft.AspNetCore.Mvc;
using StudyPlatform.Infrastructure;
using StudyPlatform.Services.Users.Interfaces;
using StudyPlatform.Web.View.Models.User;
using static StudyPlatform.Infrastructure.ClaimsPrincipalExtensions;
using static StudyPlatform.Common.ViewModelConstants.Account;
using StudyPlatform.Services.Lesson.Interfaces;

namespace StudyPlatform.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITeacherService _teacherService;
        private readonly ILessonViewService _lessonViewService;

        public AccountController(
            IUserService userService, 
            ITeacherService teacherService,
            ILessonViewService lessonViewService)
        {
            this._userService = userService;
            this._teacherService = teacherService;
            this._lessonViewService = lessonViewService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            Guid userId = User.Id();
            if (userId == null )
            {
                return NotFound();
            }

            UserAccountViewModel userModel = await this._userService.GetUserByIdAsync(userId);

            if (await this._teacherService.AnyById(userId))
            {
                userModel.Role = TeacherRoleTitle;
                userModel.Lessons = await this._lessonViewService.GetAccountCreditsAsync(userId);
            }

            return View(userModel);
        }

    }
}
