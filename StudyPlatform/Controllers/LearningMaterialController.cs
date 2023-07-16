using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyPlatform.Infrastructure;
using StudyPlatform.Services.Category.Interfaces;
using StudyPlatform.Services.Course.Interfaces;
using StudyPlatform.Services.LearningMaterial.Interfaces;
using StudyPlatform.Services.Lesson.Interfaces;
using StudyPlatform.Services.TeacherLesson.Intefaces;
using StudyPlatform.Services.Users.Interfaces;
using StudyPlatform.Web.View.Models.Lesson;

namespace StudyPlatform.Controllers
{
    // <summary>
    // controller for displaying learning material (presentations)
    // </summary>
    public class LearningMaterialController : Controller
    {
        private readonly ILearningMaterialService _learningMaterialService;
        private readonly ILearningMaterialFormService _learningMaterialFormService;
        private readonly ILessonViewService _lessonViewService;
        private readonly ICourseViewService _courseService;
        private readonly ITeacherService _teacherService;
        private readonly ITeacherLearningMaterialService _teacherLearningMaterialService;

        public LearningMaterialController(
            ILearningMaterialService learningMaterialService,
            ICourseViewService courseService,
            ILessonViewService lessonViewService,
            ILearningMaterialFormService learningMaterialFormService,
            ITeacherService teacherService,
            ITeacherLearningMaterialService teacherLearningMaterialService)
        {
            this._learningMaterialService = learningMaterialService;
            this._courseService = courseService;
            this._lessonViewService = lessonViewService;
            this._learningMaterialFormService = learningMaterialFormService;
            this._teacherService = teacherService;
            this._teacherLearningMaterialService = teacherLearningMaterialService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Upload(int id) // relevant lesson id
        {
            int courseId = await this._lessonViewService.GetCourseIdByLessonId(id);
            UploadLearningMaterialFormModel model = new UploadLearningMaterialFormModel()
            {
                LessonId = id,    
                LessonName = await this._lessonViewService.GetNameByIdAsync(id),
                CourseName = await this._courseService.GetNameByIdAsync(courseId),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(UploadLearningMaterialFormModel model)
        {
            Guid userGuid = User.Id();
            if (userGuid == Guid.Empty)
            {
                return RedirectToAction("Error", "Home", new { StatusCode = 404 });
            }

            bool isUserTeacher = await this._teacherService.AnyById(userGuid);
            if (!isUserTeacher)
            {
                return RedirectToAction("Error", "Home", new { StatusCode = 404 });
            }

            if (model.File == null)
            {
                ModelState.AddModelError(nameof(model.File), "File was not sent");
            }

            bool lmNameExists = await this._learningMaterialService.AnyByNameAsync(model.LearningMaterialName);
            if (lmNameExists)
            {
                ModelState.AddModelError(nameof(model.LearningMaterialName), "File by this name already exists.");
            }

            if (!ModelState.IsValid)
            {
                int courseId = await this._lessonViewService.GetCourseIdByLessonId(model.LessonId);
                UploadLearningMaterialFormModel getModel = new UploadLearningMaterialFormModel()
                {
                    LessonId = model.LessonId,
                    LessonName = await this._lessonViewService.GetNameByIdAsync(model.LessonId),
                    CourseName = await this._courseService.GetNameByIdAsync(courseId),
                };
                return View(getModel);
            }

            string fileName = model.File.FileName;
            string uploadPath = Path.Combine("wwwroot/files/learningmaterial/", fileName);
            using (FileStream stream = new FileStream(uploadPath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            await this._learningMaterialFormService.AddLessonAsync(model);
            int lmId = await this._learningMaterialService.GetIdByNameAsync(model.LearningMaterialName);
            await this._teacherLearningMaterialService.AddAsync(userGuid, lmId);

            // TODO: Redirect to the course
            return Ok(model.File);
        }

        [HttpGet]
        public async Task<IActionResult> ShowLearningMaterial(int id)
        {
            bool lmExists = await this._learningMaterialService.AnyByIdAsync(id);
            if (!lmExists)
            {
                return RedirectToAction("Error", "Home", new { statusCode = 404 });
            }

            LearningMaterialViewModel model = await this._learningMaterialService.GetViewModelAsync(id);

            return View(model);
        }
    }
}
