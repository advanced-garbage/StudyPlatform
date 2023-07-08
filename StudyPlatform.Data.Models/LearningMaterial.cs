using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudyPlatform.Common.ModelValidationConstants.LearningMaterial;

namespace StudyPlatform.Data.Models
{
    // class for creating learning material
    public class LearningMaterial
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [ForeignKey(nameof(Lesson))]
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}
