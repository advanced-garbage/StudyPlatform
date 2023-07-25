using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.Lesson;

namespace StudyPlatform.Web.View.Models.LearningMaterial
{
    public class LearningMaterialLessonLinkModel
    {
        public int LessonId { get; set; }

        [StringLength(NameMaxLength, MinimumLength = NameMinLength) ]
        public string LessonName { get; set; }

        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string LessonNameLink { get; set; }
    }
}
