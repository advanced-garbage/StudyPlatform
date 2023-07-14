using StudyPlatform.Web.View.Models.Lesson;
using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.Course;

namespace StudyPlatform.Web.View.Models.Course
{
    public class CourseViewModel
    {
        public CourseViewModel() 
        {
            this.Lessons = new List<LearningMaterialViewModel>();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [StringLength(DescriptionMaxLength)]
        public string? Description { get; set; }
        public int CategoryId { get; set; }

        public ICollection<LearningMaterialViewModel>? Lessons { get; set; }
    }
}
