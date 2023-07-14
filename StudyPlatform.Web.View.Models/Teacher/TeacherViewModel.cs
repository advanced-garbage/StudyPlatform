using StudyPlatform.Web.View.Models.Lesson;
using StudyPlatform.Web.View.Models.User;

namespace StudyPlatform.Web.View.Models.Teacher
{
    public class TeacherViewModel : UserViewModel
    {
        public TeacherViewModel() {
            this.Lessons = new List<LearningMaterialViewModel>();
            Role = "Teacher";
        }

        public ICollection<LearningMaterialViewModel> Lessons;
    }
}
