using StudyPlatform.Web.View.Models.Course;

namespace StudyPlatform.Services.Course.Interfaces
{
    public interface ICourseViewService
    {
        public Task<CourseViewModel> GetCourseById(int id);

        public Task EditAsync(CourseViewFormModel model);

        public Task RemoveAsync(int id);

        public Task AddAsync(CourseViewFormModel model);

        public Task AssignToNewCategoryAsync(int courseId, int categoryId); 

        public Task<CourseViewFormModel> GetFormCourseAsync(int id);

        public Task<int> GetCategoryByCourseIdAsync(int courseId);
    }
}
