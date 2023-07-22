using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyPlatform.Infrastructure;
using StudyPlatform.Infrastructure.Infrastructure;
using StudyPlatform.Services.Category.Interfaces;
using StudyPlatform.Services.Users.Interfaces;
using StudyPlatform.Web.View.Models.Category;
using System.Drawing.Text;
using static StudyPlatform.Common.ErrorMessages.Category;
using static StudyPlatform.Common.GeneralConstants;

namespace StudyPlatform.Controllers
{
    // Controller for accessing courses
    // action for all categories and for 1 certain category (by id)
    public class CategoryController : Controller
    {
        private readonly ICategoryViewService _categoryViewService;
        private readonly ICategoryViewFormService _categoryViewFormService;
        private readonly ITeacherService _teacherService;

        public CategoryController(
            ICategoryViewService categoryViewService,
            ICategoryViewFormService categoryViewFormService,
            ITeacherService teacherService)
        {
            this._categoryViewService = categoryViewService;
            this._categoryViewFormService = categoryViewFormService;
            this._teacherService = teacherService;
        }

        // TODO: Create view for "all"
        [HttpGet]
        [Route("Category/")]
        [Route("Category/Index")]
        [Route("Category/All")]
        public async Task<IActionResult> All()
        {
            AllCategoriesViewModel categories = await this._categoryViewService.GetCategoriesForAllPageAsync();

            return View(categories);
        }

        // TODO: Create "GetById" View
        [HttpGet]
        public async Task<IActionResult> GetById(int id, string categoryName)
        {
            if (!await this._categoryViewService.AnyByIdAsync(id))
            {
                return BadRequest();
            }

            var category =
                await this._categoryViewService
                .GetCategoryByIdAsync(id);

            if (categoryName != category.GetNameUrl())
            {
                return BadRequest();
            }
            return View(category);
        }


        [Authorize(Roles = $"{TeacherRoleName},{AdministratorRole}")]
        [HttpGet]
        public async Task<IActionResult> CreateCategory()
        {
            CategoryViewFormModel model = new CategoryViewFormModel();
            return View(model);
        }

        [Authorize(Roles = $"{TeacherRoleName},{AdministratorRole}")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryViewFormModel model)
        {
            if (await this._categoryViewService.AnyByNameAsync(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), CategoryByNameExists);
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            await this._categoryViewFormService.AddAsync(model);

            return RedirectToAction("All");
        }

        [Authorize(Roles = $"{TeacherRoleName},{AdministratorRole}")]
        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            if (!await this._categoryViewService.AnyByIdAsync(id))
            {
                return BadRequest();
            }

            await this._categoryViewFormService.RemoveAsync(id);
            return RedirectToAction("All");
        }

        [Authorize(Roles = $"{TeacherRoleName},{AdministratorRole}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!await this._categoryViewService.AnyByIdAsync(id))
            {
                return BadRequest();
            }

            CategoryViewFormModel model = await this._categoryViewService.GetFormCategory(id);

            return View(model);
        }

        [Authorize(Roles = $"{TeacherRoleName},{AdministratorRole}")]
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await this._categoryViewFormService.EditAsync(model);

            return RedirectToAction("All");
        }
    }
}
