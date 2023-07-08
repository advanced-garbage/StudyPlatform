using Microsoft.AspNetCore.Mvc;
using StudyPlatform.Models;


namespace StudyPlatform.Controllers
{
    public class HomeController : Controller
    {
        private static List<int> errorCodes  = new List<int>() { 400, 401 };
        public IActionResult Index()
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