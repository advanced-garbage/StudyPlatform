using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyPlatform.Data.Models;
using StudyPlatform.Infrastructure;
using StudyPlatform.Services.Course.Interfaces;
using StudyPlatform.Services.LearningMaterial.Interfaces;
using StudyPlatform.Services.Lesson.Interfaces;
using StudyPlatform.Services.TeacherLesson.Intefaces;
using StudyPlatform.Services.Users.Interfaces;
using StudyPlatform.Web.View.Models.LearningMaterial;
using static StudyPlatform.Common.ErrorMessages.LearningMaterial;
using static StudyPlatform.Common.GeneralConstants;

namespace StudyPlatform.Controllers
{
    // <summary>
    // controller for displaying learning material (presentations)
    // </summary>
    [Authorize]
    public class LearningMaterialController : Controller
    {
        private readonly ILearningMaterialService _learningMaterialService;
        private readonly ILearningMaterialFormService _learningMaterialFormService;
        private readonly ILessonViewService _lessonViewService;
        private readonly ICourseViewService _courseService;
        private readonly ITeacherService _teacherService;
        private readonly ITeacherLessonService _teacherLessonService;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;

        public LearningMaterialController(
            ILearningMaterialService learningMaterialService,
            ICourseViewService courseService,
            ILessonViewService lessonViewService,
            ILearningMaterialFormService learningMaterialFormService,
            ITeacherService teacherService,
            ITeacherLessonService teacherLessonService,
            IConfiguration config,
            UserManager<ApplicationUser> userManager)
        {
            this._learningMaterialService = learningMaterialService;
            this._courseService = courseService;
            this._lessonViewService = lessonViewService;
            this._learningMaterialFormService = learningMaterialFormService;
            this._teacherService = teacherService;
            this._teacherLessonService = teacherLessonService;
            this._config = config;
            this._userManager = userManager;
        }

        /// <summary>
        /// GET method for uploading PDF lessons.
        /// </summary>
        /// <param name="lessonId"></param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [HttpGet]
        public async Task<IActionResult> Upload(int lessonId) // relevant lesson id
        {
            ApplicationUser appUser = await this._userManager.GetUserAsync(User);
            bool isTeacher = await this._userManager.IsInRoleAsync(appUser, TeacherRoleName);
            if (!isTeacher)
            {
                return RedirectToAction("Error", "Home", new { statusCode = 401 });
            }

            int courseId = await this._lessonViewService.GetCourseIdByLessonId(lessonId);
            UploadLearningMaterialFormModel model = new UploadLearningMaterialFormModel()
            {
                LessonId = lessonId,    
                LessonName = await this._lessonViewService.GetNameByIdAsync(lessonId),
                CourseName = await this._courseService.GetNameByIdAsync(courseId),
            };

            return View(model);
        }

        /// <summary>
        /// POST method for uploading pdf files.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Upload(UploadLearningMaterialFormModel model)
        {
            ApplicationUser appUser = await this._userManager.GetUserAsync(User);
            bool isTeacher = await this._userManager.IsInRoleAsync(appUser, TeacherRoleName);
            if (!isTeacher)
            {
                return RedirectToAction("Error", "Home", new { statusCode = 401 });
            }

            Guid userGuid = User.Id();
            if (userGuid == Guid.Empty) {
                return RedirectToAction("Error", "Home", new { StatusCode = 404 });
            }

            if (model.File == null) {
                ModelState.AddModelError(nameof(model.File), FileNotSent);
            }

            bool lmNameExists = await this._learningMaterialService.AnyByNameAsync(model.LearningMaterialName);
            if (lmNameExists){
                ModelState.AddModelError(nameof(model.LearningMaterialName), FileByNameExists);
            }

            string lessonName = await this._lessonViewService.GetNameByIdAsync(model.LessonId);

            if (!ModelState.IsValid || !await this._teacherService.AnyById(userGuid))
            {
                int courseId = await this._lessonViewService.GetCourseIdByLessonId(model.LessonId);
                UploadLearningMaterialFormModel getModel = new UploadLearningMaterialFormModel(){
                    LessonId = model.LessonId,
                    LessonName = lessonName,
                    CourseName = await this._courseService.GetNameByIdAsync(courseId),
                };
                return View(getModel);
            }

            int lessonId = model.LessonId;

            try
            {
                string uploadPath = Path.Combine(this._config["FilePath:LearningMaterialPathWithRoot"], Path.GetRandomFileName());
                using (FileStream stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }
                await this._learningMaterialFormService.AddLearningMaterial(model);

                if (!await this._teacherLessonService.TeacherLessonAlreadyExists(model.LessonId, userGuid))
                {
                    await this._teacherLessonService.AddAsync(userGuid, model.LessonId);
                }
            } catch {
                return RedirectToAction("Error", "Home", new { StatusCode = 500 });
            }

            return RedirectToAction("GetLesson", "Lesson", new { id = lessonId, lessonName = lessonName.Replace(" ", "-")});
        }

        /// <summary>
        /// Returns a view for displaying a learning material entity with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ShowLearningMaterial(int id)
        {
            bool lmExists = await this._learningMaterialService.AnyByIdAsync(id);
            if (!lmExists){
                return RedirectToAction("Error", "Home", new { statusCode = 404 });
            }

            LearningMaterialViewModel model = await this._learningMaterialService.GetViewModelAsync(id);
            return View(model);
        }
    }
}
