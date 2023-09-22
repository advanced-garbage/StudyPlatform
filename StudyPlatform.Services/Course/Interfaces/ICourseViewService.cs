using StudyPlatform.Web.View.Models.Course;

namespace StudyPlatform.Services.Course.Interfaces
{
    public interface ICourseViewService
    {
        public Task<ICollection<CourseListViewModel>> GetAllAsync();
        public Task<CourseViewModel> GetById(int courseId);

        public Task<CourseViewFormModel> GetFormCourseAsync(int courseId);

        public Task<int> GetCategoryIdByCourseIdAsync(int courseId);

        public Task<string> GetNameByIdAsync(int courseId);
        public Task<int> GetIdByNameAsync(string courseName);
        public Task<string> GetNameUrlByIdAsync(int courseId);
        public Task<int> GetIdAsync(int courseId);
        Task<bool> AnyByIdAsync(int courseid);
        public Task<ICollection<CourseListViewModel>> GetAllCoursesInCategoryByCourseIdAsync(int courseId);
    }
}
