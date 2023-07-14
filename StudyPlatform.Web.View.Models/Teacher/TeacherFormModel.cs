using StudyPlatform.Web.View.Models.Lesson;
using StudyPlatform.Web.View.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Web.View.Models.Teacher
{
    public class TeacherFormModel :  UserViewModel
    {
        public TeacherFormModel() : base() {
            Role = "Teacher";
            this.Lessons = new List<LearningMaterialViewModel>();
        }
        public ICollection<LearningMaterialViewModel> Lessons;
    }
}
