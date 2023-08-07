using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StudyPlatform.Data.Models;
using StudyPlatform.Infrastructure;
using StudyPlatform.Infrastructure.Infrastructure;
using StudyPlatform.Services.Course.Interfaces;
using StudyPlatform.Services.Lesson.Interfaces;
using StudyPlatform.Services.Users.Interfaces;
using StudyPlatform.Web.View.Models.Lesson;
using static StudyPlatform.Common.ErrorMessages.Lesson;
using static StudyPlatform.Common.GeneralConstants;

namespace StudyPlatform.Controllers
{
    [Authorize]
    public class LessonController : Controller
    {
        private readonly ILessonViewService _lessonViewService;
        private readonly ILessonFormService _lessonFormService;
        private readonly ICourseViewService _courseViewService;
        private readonly ITeacherService _teacherService;
        private readonly UserManager<ApplicationUser> _userManager;

        public LessonController(
            ILessonViewService lessonViewService,
            ILessonFormService lessonFormService,
            ICourseViewService courseViewService,
            ITeacherService teacherService,
            UserManager<ApplicationUser> userManager)
        {
            this._lessonFormService = lessonFormService;
            this._lessonViewService = lessonViewService;
            this._courseViewService = courseViewService;
            this._teacherService = teacherService;
            this._userManager = userManager;
        }
        /// <summary>
        /// Returns a view displaying a lesson view model with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lessonName"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetLesson(int id, string lessonName)
        {
            if (!await this._lessonViewService.AnyByIdAsync(id))
            {
                return BadRequest();
            }

            LessonViewModel lessonsModel = await this._lessonViewService.GetLessonByIdAsync(id);

            if (lessonName != lessonsModel.GetNameUrl())
            {
                return BadRequest();
            }

            return View(lessonsModel);
        }
        /// <summary>
        /// GET method for removing a lesson entity with the specified id from the db.
        /// </summary>
        /// <param name="lessonId"></param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Remove(int lessonId)
        {
            ApplicationUser appUser = await this._userManager.GetUserAsync(User);
            bool isTeacher = await this._userManager.IsInRoleAsync(appUser, TeacherRoleName);
            if (!isTeacher)
            {
                return RedirectToAction("Index", "Home");
            }

            bool lessonExists = await this._lessonViewService.AnyByIdAsync(lessonId);
            if (!lessonExists)
            {
                return RedirectToAction("Error", "Home", new { statusCode = 404 });
            }

            int courseId = await this._lessonViewService.GetCourseIdByLessonId(lessonId);
            string courseName = await this._courseViewService.GetNameUrlByIdAsync(courseId);

            await this._lessonFormService.RemoveAsync(lessonId);

            return RedirectToAction("GetCourse", "Course", new {id = courseId, courseName = courseName});
        }

        /// <summary>
        /// GET method for editing a lesson entity with the specified id.
        /// </summary>
        /// <param name="lessonId"></param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int lessonId)
        {
            ApplicationUser appUser = await this._userManager.GetUserAsync(User);
            bool isTeacher = await this._userManager.IsInRoleAsync(appUser, TeacherRoleName);
            if (!isTeacher)
            {
                return RedirectToAction("Index", "Home");
            }

            bool lessonExists = await this._lessonViewService.AnyByIdAsync(lessonId);
            if (!lessonExists)
            {
                return RedirectToAction("Error", "Home", new { statusCode = 404 });
            }

            LessonViewFormModel model = await this._lessonFormService.GetFormByIdAsync(lessonId);

            return View(model);
        }

        /// <summary>
        /// POST method for editing a lesson with the specified id.
        /// Redirects to the GetLesson Action with the aforementioned id.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(LessonViewFormModel model)
        {
            ApplicationUser appUser = await this._userManager.GetUserAsync(User);
            bool isTeacher = await this._userManager.IsInRoleAsync(appUser, TeacherRoleName);
            if (!isTeacher)
            {
                return RedirectToAction("Index", "Home");
            }

            bool lessonExists = await this._lessonViewService.AnyByIdAsync(model.Id);
            if (!lessonExists)
            {
                return RedirectToAction("Error", "Home", new { statusCode = 404 });
            }

            if (!ModelState.IsValid)
            {
                LessonViewFormModel getModel = await this._lessonFormService.GetFormByIdAsync(model.Id);
                return View(getModel);
            }

            await this._lessonFormService.EditAsync(model);

            return RedirectToAction("GetLesson", "Lesson", new { id = model.Id, lessonName = model.GetNameUrl()});
        }

        /// <summary>
        /// GET method for adding a lesson to the DB.
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Add(int courseId)
        {
            ApplicationUser appUser = await this._userManager.GetUserAsync(User);
            bool isTeacher = await this._userManager.IsInRoleAsync(appUser, TeacherRoleName);
            if (!isTeacher)
            {
                return RedirectToAction("Index", "Home");
            }

            LessonViewFormModel model = new LessonViewFormModel()
            {
                CourseId = courseId,
                Courses = await this._courseViewService.GetAllAsync()
            };

            return View(model);
        }

        /// <summary>
        /// POST method for adding a lesson entity to the db.
        /// Redirects to the GetCourse action to the specific course in which the lesson was added.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(LessonViewFormModel model)
        {
            ApplicationUser appUser = await this._userManager.GetUserAsync(User);
            bool isTeacher = await this._userManager.IsInRoleAsync(appUser, TeacherRoleName);
            if (!isTeacher)
            {
                return RedirectToAction("Index", "Home");
            }

            bool existsByName = await this._lessonViewService.AnyByNameAsync(model.Name);
            if (existsByName)
            {
                ModelState.AddModelError(nameof(model.Name), LessonByNameExists);
            }

            if (!ModelState.IsValid)
            {
                LessonViewFormModel getModel = new LessonViewFormModel()
                {
                    Courses = await this._courseViewService.GetAllAsync()
                };
                return View(getModel);
            }

            await this._lessonFormService.AddAsync(model);

            int courseId = await this._courseViewService.GetIdAsync(model.CourseId);
            string courseName = await this._courseViewService.GetNameUrlByIdAsync(courseId);
            return RedirectToAction("GetCourse", "Course", new { id = courseId, courseName = courseName});
        }

    }
}
