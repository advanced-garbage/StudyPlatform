using StudyPlatform.Web.View.Models.Category;

namespace StudyPlatform.Services.Category.Interfaces
{
    public interface ICategoryViewService
    {
        public Task<ICollection<CategoryViewModel>> GetAllCategoriesAsync();

        public Task<CategoryViewModel> GetCategoryByIdAsync(int id);

        public Task<bool> AnyByNameAsync(string name);

        public Task AddAsync(CategoryViewFormModel model);

        public Task RemoveAsync(int id);

        public Task<CategoryViewFormModel> GetFormCategory(int id);

        public Task EditAsync(CategoryViewFormModel model);
    }
}
