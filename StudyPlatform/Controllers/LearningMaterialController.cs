using Microsoft.AspNetCore.Mvc;
using StudyPlatform.Services.Category.Interfaces;
using StudyPlatform.Services.Course.Interfaces;
using StudyPlatform.Services.LearningMaterial.Interfaces;
using StudyPlatform.Web.View.Models.Lesson;

namespace StudyPlatform.Controllers
{
    // <summary>
    // controller for displaying learning material (presentations)
    // </summary>
    public class LearningMaterialController : Controller
    {
        private readonly ILearningMaterialService _learningMaterialService;
        private readonly ICourseViewService _courseService;
        private readonly ICategoryViewService _categoryService;

        public LearningMaterialController(
            ILearningMaterialService learningMaterialService,
            ICategoryViewService categoryService,
            ICourseViewService courseService)
        {
            this._learningMaterialService = learningMaterialService;
            this._categoryService = categoryService;
            this._courseService = courseService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Upload(int id) // relevant course id
        {
            int categoryId = await this._categoryService.GetCategoryIdByCourseIdAsync(id);

            UploadLearningMaterialFormModel model = new UploadLearningMaterialFormModel()
            {
                CourseId = id,    
                CourseName = await this._courseService.GetNameByIdAsync(id),
                CategoryName = await this._categoryService.GetNameByIdAsync(categoryId)
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

            if (!ModelState.IsValid)
            {
                int categoryId = await this._categoryService.GetCategoryIdByCourseIdAsync(model.CourseId);

                UploadLearningMaterialFormModel getModel = new UploadLearningMaterialFormModel()
                {
                    CourseId = model.CourseId,
                    CourseName = await this._courseService.GetNameByIdAsync(model.CourseId),
                    CategoryName = await this._categoryService.GetNameByIdAsync(categoryId)
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

            // TODO: Redirect to the course
            return Ok(model.File);
        }
    }
}
