using Microsoft.AspNetCore.Mvc;
using StudyPlatform.Services.Users.Interfaces;
using StudyPlatform.Web.View.Models.User;
using static StudyPlatform.Common.CacheConstants;
using StudyPlatform.Services.Lesson.Interfaces;
using StudyPlatform.Services.Roles.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using StudyPlatform.Infrastructure;

namespace StudyPlatform.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILessonViewService _lessonViewService;
        private readonly IRoleService _roleService;
        private readonly IMemoryCache _memoryCache;

        public AccountController(
            IUserService userService, 
            ILessonViewService lessonViewService,
            IRoleService roleService,
            IMemoryCache memoryCache)
        {
            this._userService = userService;
            this._lessonViewService = lessonViewService;
            this._roleService = roleService;
            this._memoryCache = memoryCache;
        }

        /// <summary>
        /// Action which redirects the user to their profile.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Route("account/home")]
        //[Route("account/index")]
        //[Route("account/")]
        public async Task<IActionResult> GetProfile(string username)
        {
            try {
                Guid userId = await _userService.GetGuidByUsernameAsync(username);
                if (userId == null || userId == Guid.Empty)
                {
                    return RedirectToAction("Index", "Home");
                }

                UserAccountViewModel userModelOutput = null;
                // if the userId doesn't belong to the currently signed in user:
                if (userId != User.Id())
                {
                    userModelOutput = await GetUserModelInfoAsync(userId);
                    return View(userModelOutput);
                }

                if (!_memoryCache.TryGetValue(AccountCacheKey, out userModelOutput))
                {
                    await UpdateCache(userId);
                    userModelOutput = _memoryCache.Get<UserAccountViewModel>(AccountCacheKey);
                }

                return View(userModelOutput);
            } catch {
                return RedirectToAction("Error", "Home", new { statusCode = 500 });
            }
        }

        private async Task UpdateCache(Guid userId)
        {
            short expMins = 15;

            var expTime = DateTime.Now.AddMinutes(expMins);
            var expToken = new CancellationChangeToken(new CancellationTokenSource(TimeSpan.FromMinutes(expMins + .5)).Token);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetPriority(CacheItemPriority.Normal)
                .SetAbsoluteExpiration(expTime)
                .AddExpirationToken(expToken);

            var userModel = await GetUserModelInfoAsync(userId);

            _memoryCache.Set<UserAccountViewModel>(AccountCacheKey, userModel, cacheEntryOptions);
        }

        private async Task<UserAccountViewModel> GetUserModelInfoAsync(Guid userId)
        {
            UserAccountViewModel userModel = await this._userService.GetUserByIdAsync(userId);
            userModel.RoleTitle = await this._roleService.GetRoleNameAsync();

            if (await this._roleService.IsTeacherRole())
            {
                userModel.Lessons = await this._lessonViewService.GetAccountCreditsAsync(userId);
            }

            return userModel;
        }

    }
}
