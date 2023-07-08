using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.Category;

namespace StudyPlatform.Web.View.Models.Category
{
    public class CategoryViewFormModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Name)) { yield return new ValidationResult("Name must not be empty!"); }
        }
    }
}
