using StudyPlatform.Common;
using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.Lesson ;
using static StudyPlatform.Common.ModelValidationConstants.LearningMaterial;
using static StudyPlatform.Common.ModelValidationConstants.Course;
using Microsoft.AspNetCore.Http;

namespace StudyPlatform.Web.View.Models.Lesson
{
    public class UploadLearningMaterialFormModel : IValidatableObject
    {
        public IFormFile File { get; set; }

        [Display(Name = "Title of the material")]
        [StringLength(ModelValidationConstants.LearningMaterial.NameMaxLength,
            MinimumLength = ModelValidationConstants.LearningMaterial.NameMinLength)]
        public string LearningMaterialName { get; set; }

        [Display(Name = "Lesson Name")]
        [StringLength(ModelValidationConstants.Lesson.NameMaxLength,
            MinimumLength = ModelValidationConstants.Lesson.NameMinLength)]
        public string? LessonName { get; set; }

        [Required]
        public int LessonId { get; set; }

        [StringLength(ModelValidationConstants.Course.NameMaxLength,
            MinimumLength = ModelValidationConstants.Course.NameMinLength)]
        public string? CourseName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(LearningMaterialName)) { 
                yield return new ValidationResult("The title cannot be null!"); 
            }

            if (!File.FileName.EndsWith(".pdf")) {
                yield return new ValidationResult("You can only upload files with the .pdf extension!");
            }

        }
    }
}
