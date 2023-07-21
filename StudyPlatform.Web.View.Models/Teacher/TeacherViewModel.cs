using StudyPlatform.Web.View.Models.LearningMaterial;
using StudyPlatform.Web.View.Models.User;
using static StudyPlatform.Common.ViewModelConstants.Account;

namespace StudyPlatform.Web.View.Models.Teacher
{
    public class TeacherViewModel : UserViewModel
    {
        public TeacherViewModel() {
            this.Lessons = new List<LearningMaterialViewModel>();
            Role = TeacherRoleTitle;
        }

        public ICollection<LearningMaterialViewModel> Lessons;
    }
}
