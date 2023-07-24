using StudyPlatform.Web.View.Models.LearningMaterial.Interfaces;
using StudyPlatform.Web.View.Models.Teacher;
using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.LearningMaterial;

namespace StudyPlatform.Web.View.Models.LearningMaterial
{
    public class LearningMaterialViewModel : ILearningMaterialLinkService, IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength,
            MinimumLength = NameMinLength)]
        public string Title { get; set; }

        [Required]
        public string FileName { get; set; } = null!;

        public string FullPath { get; set; }

        public string LinkName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (LinkName.EndsWith(".pdf") || LinkName.Contains(".pdf"))
            {
                yield return new ValidationResult("Link name musn't contain the file extension!");
            }
        }
    }
}
