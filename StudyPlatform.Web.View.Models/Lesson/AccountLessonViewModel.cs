using StudyPlatform.Web.View.Models.Lesson.Interfaces;
using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.Lesson;

namespace StudyPlatform.Web.View.Models.Lesson
{
    public class AccountLessonViewModel : ILessonLink
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Lesson Name")]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        public int CourseId { get; set; }
    }
}
