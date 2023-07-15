using Microsoft.AspNetCore.Mvc;
using StudyPlatform.Services.Category.Interfaces;
using StudyPlatform.Services.Course.Interfaces;
using StudyPlatform.Services.LearningMaterial.Interfaces;
using StudyPlatform.Services.Lesson.Interfaces;
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

        public LearningMaterialController(
            ILearningMaterialService learningMaterialService,
            ICourseViewService courseService,
            ILessonViewService lessonViewService,
            ILearningMaterialFormService learningMaterialFormService)
        {
            this._learningMaterialService = learningMaterialService;
            this._courseService = courseService;
            this._lessonViewService = lessonViewService;
            this._learningMaterialFormService = learningMaterialFormService;
        }
        public IActionResult Index()
        {
            return View();
        }

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

            //TODO: Upload lesson to DB
            await this._learningMaterialFormService.AddLessonAsync(model);

            // TODO: Redirect to the course
            return Ok(model.File);
        }
    }
}
