using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.Lesson;

namespace StudyPlatform.Web.View.Models.Lesson
{
    public class AccountLessonViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        public int CourseId { get; set; }
    }
}
