using System.ComponentModel.DataAnnotations.Schema;

namespace StudyPlatform.Data.Models
{
    // Composite keys for the tables 'Teacher' and 'Lesson'
    // it has a Many-to-many relationship.
    public class TeacherLesson
    {
        [ForeignKey(nameof(Teacher))]
        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        [ForeignKey(nameof(Lesson))]
        public int LessonId { get; set; }   
        public Lesson Lesson { get; set; }
    }
}
