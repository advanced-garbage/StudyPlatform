using Microsoft.AspNetCore.Mvc;

namespace StudyPlatform.Controllers
{
    // <summary>
    // controller for displaying learning material (presentations)
    // </summary>
    public class LearningMaterialController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
