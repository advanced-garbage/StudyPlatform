using Microsoft.AspNetCore.Mvc;
using StudyPlatform.Infrastructure;
using StudyPlatform.Services.Users;
using StudyPlatform.Services.Users.Interfaces;
using StudyPlatform.Web.View.Models.Student;
using StudyPlatform.Web.View.Models.User;
using static StudyPlatform.Infrastructure.ClaimsPrincipalExtensions;


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

            UserViewModel userModel = await this._userService.GetUserByIdAsync(userId);

            // temp method where we just display the username of the user wether they're in the db or not
            if (await this._teacherService.AnyById(userId))
            {
                userModel.Role = "Teacher";
            }

            return View(userModel);
        }

    }
}
