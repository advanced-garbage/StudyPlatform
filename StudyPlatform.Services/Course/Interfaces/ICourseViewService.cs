using StudyPlatform.Web.View.Models.Course;

namespace StudyPlatform.Services.Course.Interfaces
{
    public interface ICourseViewService
    {
        public Task<ICollection<CourseListViewModel>> GetAllAsync();
        public Task<CourseViewModel> GetById(int id);

        public Task<CourseViewFormModel> GetFormCourseAsync(int id);

        public Task<int> GetCategoryIdByCourseIdAsync(int courseId);

        public Task<string> GetNameByIdAsync(int courseId); 
    }
}
