using StudyPlatform.Web.View.Models.Lesson;
using StudyPlatform.Web.View.Models.User;
using static StudyPlatform.Common.ViewModelConstants.Account;


namespace StudyPlatform.Web.View.Models.Teacher
{
    public class TeacherFormModel :  UserViewModel
    {
        public TeacherFormModel() : base() {
            this.Lessons = new List<LearningMaterialViewModel>();
            Role = TeacherRoleTitle;
        }
        public ICollection<LearningMaterialViewModel> Lessons;
    }
}
