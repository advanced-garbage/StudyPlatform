using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Data.Models
{
    public class StudentLearningMaterial 
    {
        [ForeignKey(nameof(Student))]
        public Guid StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        [ForeignKey(nameof(Lesson))]
        public int LessonId { get; set; }
        public LearningMaterial Lesson { get; set; }
    }
}
