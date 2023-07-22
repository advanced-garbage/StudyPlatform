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
        private readonly ILessonViewService _lessonViewService;

        public AccountController(
            IUserService userService, 
            ILessonViewService lessonViewService)
        {
            this._userService = userService;
            this._lessonViewService = lessonViewService;
        }

        [HttpGet]
        [Route("account/home")]
        [Route("account/index")]
        [Route("account/")]
        public async Task<IActionResult> GetProfile()
        {
            Guid userId = User.Id();
            if (userId == null )
            {
                return NotFound();
            }

            UserAccountViewModel userModel = await this._userService.GetUserByIdAsync(userId);
            userModel.Role = User.GetRole();
            if (User.IsTeacher())
            {
                userModel.Lessons = await this._lessonViewService.GetAccountCreditsAsync(userId);
            }

            return View(userModel);
        }

    }
}
