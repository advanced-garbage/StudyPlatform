using Microsoft.AspNetCore.Mvc;
using StudyPlatform.Services.Category.Interfaces;
using StudyPlatform.Services.Course.Interfaces;
using StudyPlatform.Web.View.Models.Course;
using Microsoft.AspNetCore.Authorization;
using StudyPlatform.Services.Users.Interfaces;
using StudyPlatform.Infrastructure;
using static StudyPlatform.Common.GeneralConstants;
using StudyPlatform.Infrastructure.Infrastructure;

namespace StudyPlatform.Controllers
{
    [Authorize]
    // a method for displaying and accessing courses
    public class CourseController : Controller
    {
        private readonly ICourseViewService _courseViewService;
        private readonly ICategoryViewService _categoryViewService;
        private readonly ICourseViewFormService _courseViewFormService;
        private readonly ITeacherService _teacherService;

        public CourseController(
            ICourseViewService courseViewService,
            ICategoryViewService categoryViewService,
            ICourseViewFormService courseViewFormService,
            ITeacherService teacherService)
        {
            this._courseViewService = courseViewService;
            this._categoryViewService = categoryViewService;
            this._courseViewFormService = courseViewFormService;
            this._teacherService = teacherService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCourse(int id, string courseName)
        {
            var course = await this._courseViewService.GetById(id);
            if (course == null) {
                return RedirectToAction("Error", "Home");
            }

            if(courseName != course.GetNameUrl())
            {
                return BadRequest();
            }

            if (User.IsTeacher()) {
                course.isViewedByTeacher = true;
            }
            
            return View(course);
        }

        [Authorize(Roles = TeacherRoleName)]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            CourseViewFormModel model = await this._courseViewService.GetFormCourseAsync(id);
            model.Categories = await this._categoryViewService.GetAllCategoriesAsync();

            return View(model);
        }

        [Authorize(Roles = TeacherRoleName)]
        [HttpPost]
        public async Task<IActionResult> Edit(CourseViewFormModel model)
        {
            if (!ModelState.IsValid){
                return View();
            }

            await this._courseViewFormService.EditAsync(model);

            return RedirectToAction("GetCourse", new { id = model.Id});
        }

        [Authorize(Roles = TeacherRoleName)]
        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            await this._courseViewFormService.RemoveAsync(id);
            // TODO: Return to the category of the deleted course
            return RedirectToAction("All", "Category");
        }

        [Authorize(Roles = TeacherRoleName)]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            CourseViewFormModel model = new CourseViewFormModel() {
                Categories = await this._categoryViewService.GetAllCategoriesAsync()
            };

            return View(model);
        }

        [Authorize(Roles = TeacherRoleName)]
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
