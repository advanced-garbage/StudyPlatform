using Microsoft.AspNetCore.Mvc;
using StudyPlatform.Services.Users;
using StudyPlatform.Services.Users.Interfaces;
using StudyPlatform.Web.View.Models.Student;
using System.Security.Claims;

namespace StudyPlatform.Controllers
{
    public class AccountController : Controller
    {
        private string userId = string.Empty;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        public AccountController(
            IStudentService studentService, 
            ITeacherService teacherService)
        {
            _studentService = studentService;
            _teacherService = teacherService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            StudentViewModel studentModel = await this._studentService.GetStudentAsync(userId);

            return View(studentModel);
        }

        protected string GetUserId()
        {
            string userId = string.Empty;

            if (User != null)
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return userId;
        }
    }
}
