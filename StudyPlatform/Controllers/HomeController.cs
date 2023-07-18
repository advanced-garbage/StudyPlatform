using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyPlatform.Data.Models;
using StudyPlatform.Models;


namespace StudyPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private static IList<string> _roleNames;
        private static IList<int> errorCodes  = new List<int>() { 400, 401 };
        public HomeController(
            RoleManager<IdentityRole<Guid>> roleManager,
            IConfiguration config,
            UserManager<ApplicationUser> userManager)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
            this._config = config;
            _roleNames = new List<string>() 
            { this._config["RoleNames:TeacherRoleName"],
              this._config["RoleNames:StudentRoleName"],
              this._config["RoleNames:AdministratorRoleName"] };
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Error(int statusCode)
        {
            if (errorCodes.Contains(statusCode))
            {
                return View($"Error{statusCode}");
            }

            return View("Error");
        }
    }
}