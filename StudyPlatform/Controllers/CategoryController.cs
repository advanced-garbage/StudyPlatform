using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using StudyPlatform.Infrastructure.Infrastructure;
using StudyPlatform.Services.Category.Interfaces;
using StudyPlatform.Services.Users.Interfaces;
using StudyPlatform.Web.View.Models.Category;
using static StudyPlatform.Common.ErrorMessages.Category;
using static StudyPlatform.Common.GeneralConstants;
using static StudyPlatform.Common.CacheConstants;

namespace StudyPlatform.Controllers
{
    // Controller for accessing courses
    // action for all categories and for 1 certain category (by id)
    public class CategoryController : Controller
    {
        private readonly ICategoryViewService _categoryViewService;
        private readonly ICategoryViewFormService _categoryViewFormService;
        private readonly IMemoryCache _memoryCache;

        public CategoryController(
            ICategoryViewService categoryViewService,
            ICategoryViewFormService categoryViewFormService,
            IMemoryCache memoryCache)
        {
            this._categoryViewService = categoryViewService;
            this._categoryViewFormService = categoryViewFormService;
            this._memoryCache = memoryCache;
        }

        /// <summary>
        /// Action which redirects to the page for displaying every category by name.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> All()
        {
            AllCategoriesViewModel categories = await this._categoryViewService.GetCategoriesForAllPageAsync();

            return View(categories);
        }

        /// <summary>
        /// Action which redirects to the page for displaying every category by name.
        /// Caches the call for 3 minutes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Category/")]
        [Route("Category/Index")]
        [Route("Category/All")]
        public async Task<IActionResult> AllCache()
        {
            if (!_memoryCache.TryGetValue(AllCategoriesCacheKey, out AllCategoriesViewModel allCategoriesCache))
            {
                allCategoriesCache = await this._categoryViewService.GetCategoriesForAllPageAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(3));

                _memoryCache.Set(AllCategoriesCacheKey, allCategoriesCache, cacheEntryOptions);
            }

            return View("All", allCategoriesCache);
        }

        /// <summary>
        /// Returns a view containing info for a Category model with the specified id and name.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// GET method for adding a Category Entity to DB.
        /// </summary>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [HttpGet]
        [Authorize(Roles = $"{TeacherRoleName},{AdministratorRoleName}")]
        public async Task<IActionResult> CreateCategory()
        {
            CategoryViewFormModel model = new CategoryViewFormModel();
            return View(model);
        }

        /// <summary>
        /// POST method for adding a Category Entity to DB.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = $"{TeacherRoleName},{AdministratorRoleName}")]
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

            return RedirectToAction("All", "Category");
        }

        /// <summary>
        /// GET method for removing category entity with the specified id from DB.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = $"{TeacherRoleName},{AdministratorRoleName}")]
        [HttpGet]
        public async Task<IActionResult> Remove(int categoryId)
        {
            if (!await this._categoryViewService.AnyByIdAsync(categoryId))
            {
                return BadRequest();
            }

            await this._categoryViewFormService.RemoveAsync(categoryId);
            return RedirectToAction("All", "Category");
        }

        /// <summary>
        /// GET method for editing a category entity with the specified id.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = $"{TeacherRoleName},{AdministratorRoleName}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int categoryId)
        {
            if (!await this._categoryViewService.AnyByIdAsync(categoryId))
            {
                return BadRequest();
            }

            CategoryViewFormModel model = await this._categoryViewService.GetFormCategory(categoryId);

            return View(model);
        }

        /// <summary>
        /// POST method for editing a category entity with the specified id.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = $"{TeacherRoleName},{AdministratorRoleName}")]
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await this._categoryViewFormService.EditAsync(model);

            return RedirectToAction("All", "Category");
        }
    }
}
