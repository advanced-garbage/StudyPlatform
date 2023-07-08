
using StudyPlatform.Web.View.Models.Category;
using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.Course;

namespace StudyPlatform.Web.View.Models.Course
{
    public class CourseViewFormModel : IValidatableObject
    {
        public CourseViewFormModel() { 
            this.Categories = new List<CategoryViewModel>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; }
        public int CategoryId { get; set; }  
        public ICollection<CategoryViewModel> Categories { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Name)) { yield return new ValidationResult("Name must not be null!"); }
        }  
    }
}
