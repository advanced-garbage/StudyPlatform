using Microsoft.AspNetCore.Mvc;
using StudyPlatform.Services.Category.Interfaces;
using StudyPlatform.Services.Course.Interfaces;
using StudyPlatform.Web.View.Models.Course;
using Microsoft.AspNetCore.Authorization;
using StudyPlatform.Services.Users.Interfaces;
using StudyPlatform.Infrastructure;
using static StudyPlatform.Common.GeneralConstants;
using StudyPlatform.Infrastructure.Infrastructure;
using StudyPlatform.Data.Models;
using Microsoft.AspNetCore.Identity;
using static StudyPlatform.Common.GeneralConstants;

namespace StudyPlatform.Controllers
{
    [Authorize]
    // a method for displaying and accessing courses
    public class CourseController : Controller
    {
        private readonly ICourseViewService _courseViewService;
        private readonly ICategoryViewService _categoryViewService;
        private readonly ICourseViewFormService _courseViewFormService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseController(
            ICourseViewService courseViewService,
            ICategoryViewService categoryViewService,
            ICourseViewFormService courseViewFormService,
            UserManager<ApplicationUser> userManager)
        {
            this._courseViewService = courseViewService;
            this._categoryViewService = categoryViewService;
            this._courseViewFormService = courseViewFormService;
            this._userManager = userManager;
        }

        /// <summary>
        /// Returns a course view with the specified id and name.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="courseName"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
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

            return View(course);
        }

        /// <summary>
        /// GET method for editing a course entity with the specified id
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int courseId)
        {
            ApplicationUser appUser = await this._userManager.GetUserAsync(User);
            bool isTeacher = await this._userManager.IsInRoleAsync(appUser, TeacherRoleName);
            if (!isTeacher)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!await this._courseViewService.AnyByIdAsync(courseId))
            {
                return BadRequest();
            }

            CourseViewFormModel model = await this._courseViewService.GetFormCourseAsync(courseId);

            return View(model);
        }

        /// <summary>
        /// POST method for editing a course entity with the specified id.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(CourseViewFormModel model)
        {
            ApplicationUser appUser = await this._userManager.GetUserAsync(User);
            bool isTeacher = await this._userManager.IsInRoleAsync(appUser, TeacherRoleName);
            if (!isTeacher)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid){
                return View();
            }

            await this._courseViewFormService.EditAsync(model);

            return RedirectToAction("GetCourse", new { id = model.Id, courseName = model.GetNameUrl() });
        }

        /// <summary>
        /// GET method for removing a course with the specified id. Returns a view to GetById (Category)
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Remove(int courseId)
        {
            ApplicationUser appUser = await this._userManager.GetUserAsync(User);
            bool isTeacher = await this._userManager.IsInRoleAsync(appUser, TeacherRoleName);
            if (!isTeacher)
            {
                return RedirectToAction("Index", "Home");
            }

            int categoryId = await this._categoryViewService.GetCategoryIdByCourseIdAsync(courseId);
            string categoryName = await this._categoryViewService.GetNameUrlByIdAsync(categoryId);
            await this._courseViewFormService.RemoveAsync(courseId);

            return RedirectToAction("GetById", "Category", new {id = categoryId, categoryName = categoryName});
        }

        /// <summary>
        /// GET method for adding a course entity.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Add(int categoryId)
        {
            ApplicationUser appUser = await this._userManager.GetUserAsync(User);
            bool isTeacher = await this._userManager.IsInRoleAsync(appUser, TeacherRoleName);
            if (!isTeacher)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!await this._categoryViewService.AnyByIdAsync(categoryId))
            {
                return BadRequest();
            }

            CourseViewFormModel model = new CourseViewFormModel() {
                Categories = await this._categoryViewService.GetAllCategoriesAsync(),
                CategoryId = categoryId
            };

            return View(model);
        }

        /// <summary>
        /// POST method for adding a course entity. Redirects to the GetById with the id from the newly added course entity.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Add(CourseViewFormModel model)
        {
            ApplicationUser appUser = await this._userManager.GetUserAsync(User);
            bool isTeacher = await this._userManager.IsInRoleAsync(appUser, TeacherRoleName);
            if (!isTeacher)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            await this._courseViewFormService.AddAsync(model);
            int courseId = await this._courseViewService.GetIdByNameAsync(model.Name);
            return RedirectToAction("GetCourse", new { id = courseId, courseName = model.GetNameUrl() });
        }
    }
}
