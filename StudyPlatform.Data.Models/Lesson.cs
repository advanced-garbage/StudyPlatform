using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static StudyPlatform.Common.ModelValidationConstants.Lesson;

namespace StudyPlatform.Data.Models
{
    // lesson class for storing learning material, homework and quizes
    public class Lesson
    {
        public Lesson() { 
            this.Teachers = new List<Teacher>();
        }
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        // teachers are considred authors of lessons.
        // called "teachers" for predictability.
        public ICollection<Teacher> Teachers { get; set; }
    }
}
