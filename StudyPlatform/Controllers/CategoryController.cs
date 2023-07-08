using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyPlatform.Services.Category.Interfaces;
using StudyPlatform.Web.View.Models.Category;

namespace StudyPlatform.Controllers
{
    // Controller for accessing courses
    // action for all categories and for 1 certain category (by id)
    public class CategoryController : Controller
    {
        private readonly ICategoryViewService _categoryViewService;
        public CategoryController(
            ICategoryViewService categoryViewService)
        {
            this._categoryViewService = categoryViewService;
        }

        public IActionResult Index()
        {
            // if we somehow get here
            return RedirectToAction("All");
        }

        // TODO: Create view for "all"
        [HttpGet]
        [Route("Category/")]
        [Route("Category/Index")]
        [Route("Category/All")]
        public async Task<IActionResult> All()
        {
            var categories = await this._categoryViewService
                .GetAllCategoriesAsync();

            return View(categories);
        }

        // TODO: Create "GetById" View
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var category = 
                await this._categoryViewService
                .GetCategoryByIdAsync(id);

            return View(category);
        }


        [HttpGet]
        public async Task<IActionResult> CreateCategory()
        {
            CategoryViewFormModel model = new CategoryViewFormModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryViewFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (await this._categoryViewService.AnyByNameAsync(model.Name))
            {
                ModelState.AddModelError(model.Name, $"Object with this name ({model.Name}) already exists in the DataBase.");
                return View();
            }

            await this._categoryViewService.AddAsync(model);

            return RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            await this._categoryViewService.RemoveAsync(id);
            return RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            CategoryViewFormModel model = await this._categoryViewService.GetFormCategory(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await this._categoryViewService.EditAsync(model);

            return RedirectToAction("All");
        }

    }
}
