using Microsoft.AspNetCore.Mvc;
using StudyPlatform.Services;
using StudyPlatform.Web.View.Models.Category;
using StudyPlatform.Services.Category.Interfaces;
using StudyPlatform.Services.Course.Interfaces;
using StudyPlatform.Web.View.Models.Course;
using Microsoft.AspNetCore.Authorization;

namespace StudyPlatform.Controllers
{
    // a method for displaying and accessing courses
    public class CourseController : Controller
    {
        private readonly ICourseViewService _courseViewService;
        private readonly ICategoryViewService _categoryViewService;
        private readonly ICourseViewFormService _courseViewFormService;

        public CourseController(
            ICourseViewService courseViewService,
            ICategoryViewService categoryViewService,
            ICourseViewFormService courseViewFormService)
        {
            this._courseViewService = courseViewService;
            this._categoryViewService = categoryViewService;
            this._courseViewFormService = courseViewFormService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("GetCourse");
        }

        [HttpGet]
        public async Task<IActionResult> GetCourse(int id)
        {
            var course = await this._courseViewService.GetById(id);
            if (course == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(course);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            CourseViewFormModel model = await this._courseViewService.GetFormCourseAsync(id);
            model.Categories = await this._categoryViewService.GetAllCategoriesAsync();

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(CourseViewFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await this._courseViewFormService.EditAsync(model);

            return RedirectToAction("GetCourse", new { id = model.Id});
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            await this._courseViewFormService.RemoveAsync(id);
            // TODO: Return to the category of the deleted course
            return RedirectToAction("All", "Category");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            CourseViewFormModel model = new CourseViewFormModel()
            {
                Categories = await this._categoryViewService.GetAllCategoriesAsync()
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CourseViewFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await this._courseViewFormService.AddAsync(model);
            return RedirectToAction("GetCourse", new { id = model.Id });
        }
    }
}
