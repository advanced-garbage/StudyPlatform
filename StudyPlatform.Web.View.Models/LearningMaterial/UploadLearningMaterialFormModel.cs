using StudyPlatform.Common;
using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.Category;
using static StudyPlatform.Common.ModelValidationConstants.LearningMaterial;
using static StudyPlatform.Common.ModelValidationConstants.Course;
using Microsoft.AspNetCore.Http;

namespace StudyPlatform.Web.View.Models.Lesson
{
    public class UploadLearningMaterialFormModel : IValidatableObject
    {
        public IFormFile File { get; set; }

        [Display(Name = "Lesson Name")]
        [StringLength(ModelValidationConstants.LearningMaterial.NameMaxLength,
            MinimumLength = ModelValidationConstants.LearningMaterial.NameMinLength)]
        public string LessonName { get; set; }

        [StringLength(ModelValidationConstants.Category.NameMaxLength, 
            MinimumLength = ModelValidationConstants.Category.NameMinLength)]
        public string? CategoryName { get; set; }

        [StringLength(ModelValidationConstants.Course.NameMaxLength,
            MinimumLength = ModelValidationConstants.Course.NameMinLength)]
        public string? CourseName { get; set; }
        [Required]
        public int CourseId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(LessonName)) { yield return new ValidationResult("Lesson Name cannot be null!"); }
        }
    }
}
