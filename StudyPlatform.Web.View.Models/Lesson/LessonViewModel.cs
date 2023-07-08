using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudyPlatform.Common.ModelValidationConstants.Lesson;

namespace StudyPlatform.Web.View.Models.Lesson
{
    public class LessonViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [StringLength(DescriptionMaxLength)]
        public string? Description { get; set; }
        public int CourseId { get; set; }
    }
}
