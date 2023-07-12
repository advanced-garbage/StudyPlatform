using Microsoft.AspNetCore.Mvc;
using StudyPlatform.Infrastructure;
using StudyPlatform.Services.Users.Interfaces;
using StudyPlatform.Web.View.Models.User;
using static StudyPlatform.Infrastructure.ClaimsPrincipalExtensions;
using static StudyPlatform.Common.ViewModelConstants.Account;

namespace StudyPlatform.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITeacherService _teacherService;
        public AccountController(
            IUserService userService, 
            ITeacherService teacherService)
        {
            _userService = userService;
            _teacherService = teacherService;
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
                // TODO: Add lessons written by this teacher
            }

            return View(userModel);
        }

    }
}
