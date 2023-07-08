using StudyPlatform.Web.View.Models.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Web.View.Models.Teacher
{
    public class TeacherViewModel
    {
        public TeacherViewModel() {
            this.Lessons = new List<LessonViewModel>();
        }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Age { get; set; }

        public ICollection<LessonViewModel> Lessons;
    }
}
