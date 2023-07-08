using Microsoft.AspNetCore.Mvc;

namespace StudyPlatform.Controllers
{
    // <summary>
    // controller for handling homework/quiz models and views
    // </summary>
    public class HomeworkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
