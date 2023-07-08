using StudyPlatform.Web.View.Models.Lesson;
using StudyPlatform.Web.View.Models.User;

namespace StudyPlatform.Web.View.Models.Teacher
{
    public class TeacherViewModel : UserViewModel
    {
        public TeacherViewModel() {
            this.Lessons = new List<LessonViewModel>();
            Role = "Teacher";
        }

        public ICollection<LessonViewModel> Lessons;
    }
}
