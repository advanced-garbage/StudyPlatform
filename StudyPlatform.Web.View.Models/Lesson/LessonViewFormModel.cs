using static StudyPlatform.Common.ModelValidationConstants.Lesson;
using System.ComponentModel.DataAnnotations;
using StudyPlatform.Web.View.Models.Course;
using StudyPlatform.Web.View.Models.Lesson.Interfaces;

namespace StudyPlatform.Web.View.Models.Lesson
{
    public class LessonViewFormModel : ILessonLink
    {
        public LessonViewFormModel()
        {
            this.Courses = new List<CourseListViewModel>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength,
            MinimumLength = NameMinLength)]
        [Display(Name = "Lesson Name")]
        public string Name { get; set; } = null!;

        [Display(Name = "Lesson Description")]
        [StringLength(DescriptionMaxLength)]
        public string? Description { get; set; }
        public int CourseId { get; set; }
        public ICollection<CourseListViewModel>? Courses;
    }
}
