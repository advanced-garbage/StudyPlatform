using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public LearningMaterialController(
            ILearningMaterialService learningMaterialService,
            ICourseViewService courseService,
            ILessonViewService lessonViewService,
            ILearningMaterialFormService learningMaterialFormService,
            ITeacherService teacherService,
            ITeacherLessonService teacherLessonService,
            IConfiguration config)
        {
            this._learningMaterialService = learningMaterialService;
            this._courseService = courseService;
            this._lessonViewService = lessonViewService;
            this._learningMaterialFormService = learningMaterialFormService;
            this._teacherService = teacherService;
            this._teacherLessonService = teacherLessonService;
            this._config = config;
        }

        /// <summary>
        /// GET method for uploading PDF lessons.
        /// </summary>
        /// <param name="lessonId"></param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = $"{TeacherRoleName},{AdministratorRoleName}")]
        [HttpGet]
        public async Task<IActionResult> Upload(int lessonId) // relevant lesson id
        {
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
        [Authorize(Roles = $"{TeacherRoleName},{AdministratorRoleName}")]
        [HttpPost]
        public async Task<IActionResult> Upload(UploadLearningMaterialFormModel model)
        {
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

            if (!ModelState.IsValid || !await this._teacherService.AnyById(userGuid))
            {
                int courseId = await this._lessonViewService.GetCourseIdByLessonId(model.LessonId);
                UploadLearningMaterialFormModel getModel = new UploadLearningMaterialFormModel(){
                    LessonId = model.LessonId,
                    LessonName = await this._lessonViewService.GetNameByIdAsync(model.LessonId),
                    CourseName = await this._courseService.GetNameByIdAsync(courseId),
                };
                return View(getModel);
            }

            string fileName = model.File.FileName;
            string uploadPath = Path.Combine(this._config["FilePath:LearningMaterialPathWithRoot"], fileName);
            using (FileStream stream = new FileStream(uploadPath, FileMode.Create)) {
                await model.File.CopyToAsync(stream);
            }

            await this._learningMaterialFormService.AddLearningMaterial(model);

            if (!await this._teacherLessonService.TeacherLessonAlreadyExists(model.LessonId, userGuid))
            {
                await this._teacherLessonService.AddAsync(userGuid, model.LessonId);
            }

            // TODO: Redirect to the course
            return Ok(model.File);
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
