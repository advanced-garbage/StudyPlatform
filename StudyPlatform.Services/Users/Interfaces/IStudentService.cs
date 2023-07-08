
using StudyPlatform.Web.View.Models.Student;

namespace StudyPlatform.Services.Users.Interfaces
{
    public interface IStudentService
    {
        public Task<StudentViewModel> GetStudentAsync(string userId);
    }
}
