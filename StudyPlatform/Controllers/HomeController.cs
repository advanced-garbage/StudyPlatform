using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyPlatform.Data.Models;
using StudyPlatform.Infrastructure;
using StudyPlatform.Models;


namespace StudyPlatform.Controllers
{
    public class HomeController : Controller
    {
        private IEnumerable<int> errorCodes;
        public HomeController()
        {
            errorCodes = new List<int>() { 400, 401 };
        }

        public async Task<IActionResult> Index()
        {
            // DELETE!
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