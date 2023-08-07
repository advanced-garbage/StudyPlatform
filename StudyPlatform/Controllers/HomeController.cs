using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyPlatform.Data.Models;
using StudyPlatform.Infrastructure;
using StudyPlatform.Models;
using StudyPlatform.Services.Users;

namespace StudyPlatform.Controllers
{
    public class HomeController : Controller
    {
        private IEnumerable<int> errorCodes;
        public HomeController()
        {
            errorCodes = new List<int>() { 400, 401 };
        }

        /// <summary>
        /// Returns the index view for this web project.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View();
        }

        /// <summary>
        /// Returns an error view depending on the status code.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
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