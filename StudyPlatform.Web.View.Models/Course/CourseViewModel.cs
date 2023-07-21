using StudyPlatform.Web.View.Models.Course.Interfaces;
using StudyPlatform.Web.View.Models.Lesson;
using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.Course;

namespace StudyPlatform.Web.View.Models.Course
{
    public class CourseViewModel : ICourseLink
    {
        public CourseViewModel() 
        {
            this.Lessons = new List<LessonViewModel>();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [StringLength(DescriptionMaxLength)]
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public bool? isViewedByTeacher { get; set; }
        public ICollection<LessonViewModel>? Lessons { get; set; }
    }
}
