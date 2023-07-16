
using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.LearningMaterial;

namespace StudyPlatform.Web.View.Models.Lesson
{
    public class LearningMaterialViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength,
            MinimumLength = NameMinLength)]
        public string Title { get; set; }

        [Required]
        public string FileName { get; set; } = null!;

        public string FullPath { get; set; }

    }
}
