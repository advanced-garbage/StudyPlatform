using StudyPlatform.Common;
using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.Lesson;
using static StudyPlatform.Common.ModelValidationConstants.LearningMaterial;
using static StudyPlatform.Common.ModelValidationConstants.Course;
using Microsoft.AspNetCore.Http;

namespace StudyPlatform.Web.View.Models.LearningMaterial
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

        private static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(LearningMaterialName))
            {
                yield return new ValidationResult("The title cannot be null!");
            }

            if (!File.FileName.EndsWith(".pdf"))
            {
                yield return new ValidationResult("You can only upload files with the .pdf extension!");
            }

            if (File.Length == 0)
            {
                yield return new ValidationResult("File cannot be empty!");
            }

            if (ConvertBytesToMegabytes(File.Length) > 5)
            {
                yield return new ValidationResult("File cannot be bigger than 5MBs!");
            }
        }
    }
}
