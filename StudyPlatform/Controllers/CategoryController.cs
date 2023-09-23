using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using StudyPlatform.Infrastructure.Infrastructure;
using StudyPlatform.Services.Category.Interfaces;
using StudyPlatform.Web.View.Models.Category;
using static StudyPlatform.Common.ErrorMessages.Category;
using static StudyPlatform.Common.GeneralConstants;
using static StudyPlatform.Common.CacheConstants;
using Microsoft.AspNetCore.Identity;
using StudyPlatform.Data.Models;
using Microsoft.Extensions.Primitives;

namespace StudyPlatform.Controllers
{
    // Controller for accessing courses
    // action for all categories and for 1 certain category (by id)
    public class CategoryController : Controller
    {
        private readonly ICategoryViewService _categoryViewService;
        private readonly ICategoryViewFormService _categoryViewFormService;
        private readonly IMemoryCache _memoryCache;
        private readonly UserManager<ApplicationUser> _userManager;
        public CategoryController(
            ICategoryViewService categoryViewService,
            ICategoryViewFormService categoryViewFormService,
            IMemoryCache memoryCache,
            UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
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
            if (!_memoryCache.TryGetValue(AllCategoriesCacheKey, out AllCategoriesViewModel allCategoriesOutput))
            {
                await UpdateCacheAsync();
                allCategoriesOutput = _memoryCache.Get<AllCategoriesViewModel>(AllCategoriesCacheKey);
            }

            return View("All", allCategoriesOutput);
        }

        private void CacheItemRemoved(object key, object value, EvictionReason reason, object state)
        {
            Console.WriteLine(string.Format("EvictionCallback: Cache with key {0} has expired.  */", key));
        }

        private async Task UpdateCacheAsync()
        {
            AllCategoriesViewModel allCategoriesOutput = null;
            short expMins = 3;

            var expTime = DateTime.Now.AddMinutes(expMins);
            var expToken = new CancellationChangeToken(new CancellationTokenSource(TimeSpan.FromMinutes(expMins + .5)).Token);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetPriority(CacheItemPriority.Normal)
                .SetAbsoluteExpiration(expTime)
                .AddExpirationToken(expToken)
                .RegisterPostEvictionCallback(callback: CacheItemRemoved, state: this);

            allCategoriesOutput = await this._categoryViewService.GetCategoriesForAllPageAsync();

            _memoryCache.Set(AllCategoriesCacheKey, allCategoriesOutput, cacheEntryOptions);
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
                return RedirectToAction("Error", "Home", new { statusCode = 401 });
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
        [Authorize]
        public async Task<IActionResult> CreateCategory()
        {
            ApplicationUser appUser = await this._userManager.GetUserAsync(User);
            bool isTeacher = await this._userManager.IsInRoleAsync(appUser, TeacherRoleName);
            if (!isTeacher)
            {
                return RedirectToAction("Error", "Home", new { statusCode = 401 });
            }

            CategoryViewFormModel model = new CategoryViewFormModel();
            return View(model);
        }

        /// <summary>
        /// POST method for adding a Category Entity to DB.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryViewFormModel model)
        {
            ApplicationUser appUser = await this._userManager.GetUserAsync(User);
            bool isTeacher = await this._userManager.IsInRoleAsync(appUser, TeacherRoleName);
            if (!isTeacher)
            {
                return RedirectToAction("Error", "Home", new { statusCode = 401 });
            }

            if (await this._categoryViewService.AnyByNameAsync(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), CategoryByNameExists);
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                await this._categoryViewFormService.AddAsync(model);
                await UpdateCacheAsync();
            } 
            catch 
            {
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }

            return RedirectToAction("All", "Category");
        }

        /// <summary>
        /// GET method for removing category entity with the specified id from DB.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Remove(int categoryId)
        {
            ApplicationUser appUser = await this._userManager.GetUserAsync(User);
            bool isTeacher = await this._userManager.IsInRoleAsync(appUser, TeacherRoleName);
            if (!isTeacher)
            {
                return RedirectToAction("Error", "Home", new { statusCode = 401 });
            }

            if (!await this._categoryViewService.AnyByIdAsync(categoryId))
            {
                return BadRequest();
            }

            try
            {
                await this._categoryViewFormService.RemoveAsync(categoryId);
                await UpdateCacheAsync();
            }
            catch
            {
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }

            return RedirectToAction("All", "Category");
        }

        /// <summary>
        /// GET method for editing a category entity with the specified id.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int categoryId)
        {
            ApplicationUser appUser = await this._userManager.GetUserAsync(User);
            bool isTeacher = await this._userManager.IsInRoleAsync(appUser, TeacherRoleName);
            if (!isTeacher)
            {
                return RedirectToAction("Error", "Home", new { statusCode = 401 });
            }

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
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewFormModel model)
        {
            ApplicationUser appUser = await this._userManager.GetUserAsync(User);
            bool isTeacher = await this._userManager.IsInRoleAsync(appUser, TeacherRoleName);
            if (!isTeacher)
            {
                return RedirectToAction("Error", "Home", new { statusCode = 401 });
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                await this._categoryViewFormService.EditAsync(model);
                await UpdateCacheAsync();
            }
            catch
            {
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }

            return RedirectToAction("All", "Category");
        }
    }
}
