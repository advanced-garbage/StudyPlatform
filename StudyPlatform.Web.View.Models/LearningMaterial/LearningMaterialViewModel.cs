
using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.Lesson;

namespace StudyPlatform.Web.View.Models.Lesson
{
    public class LearningMaterialViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string FileName { get; set; } = null!;

    }
}
