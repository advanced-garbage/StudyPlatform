using StudyPlatform.Web.View.Models.Category;
using StudyPlatform.Web.View.Models.Course;

namespace StudyPlatform.Services.Category.Interfaces
{
    public interface ICategoryViewService
    {
        public Task<ICollection<CategoryViewModel>> GetAllCategoriesAsync();

        public Task<int> GetCategoryIdByCourseIdAsync(int courseId);

        public Task<CategoryViewModel> GetCategoryByIdAsync(int id);

        public Task<bool> AnyByNameAsync(string name);
        public Task<bool> AnyByIdAsync(int id);

        public Task<CategoryViewFormModel> GetFormCategory(int id);

        public Task<string> GetNameByIdAsync(int categoryId);

        public Task<string> GetNameUrlByIdAsync(int categoryId);

        public Task<AllCategoriesViewModel> GetCategoriesForAllPageAsync();

    }
}
