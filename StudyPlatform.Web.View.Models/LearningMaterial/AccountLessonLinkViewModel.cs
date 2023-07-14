using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.Lesson;
using static StudyPlatform.Common.ModelValidationConstants.Course;
using static StudyPlatform.Common.ModelValidationConstants.Category;

namespace StudyPlatform.Web.View.Models.Lesson
{
    // Move to Lessons folder?
    public class AccountLessonLinkViewModel
    {
        [Required]
        public int LessonId { get; set; }

        [Required]
        [StringLength(Common.ModelValidationConstants.Lesson.NameMaxLength,
            MinimumLength = Common.ModelValidationConstants.Lesson.NameMinLength)]
        public string LessonName { get; set; } = null!;

        [Required]
        public int CourseId { get; set; }

        [Required]
        [StringLength(Common.ModelValidationConstants.Course.NameMaxLength,
            MinimumLength = Common.ModelValidationConstants.Course.NameMinLength)]
        public string CourseName { get; set; } = null!;

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(Common.ModelValidationConstants.Category.NameMaxLength,
            MinimumLength = Common.ModelValidationConstants.Category.NameMinLength)]
        public string CategoryName { get; set; } = null!;

    }
}
