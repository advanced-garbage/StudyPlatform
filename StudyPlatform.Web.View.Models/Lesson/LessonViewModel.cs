using System.ComponentModel.DataAnnotations;
using StudyPlatform.Web.View.Models.LearningMaterial;
using StudyPlatform.Web.View.Models.Lesson.Interfaces;
using static StudyPlatform.Common.ModelValidationConstants.Lesson;

namespace StudyPlatform.Web.View.Models.Lesson
{
    public class LessonViewModel : ILessonLink
    {
        public LessonViewModel() {
            this.LearningMaterials = new List<LearningMaterialViewModel>();
        }
        public int Id { get; set; }

        [Display(Name = "Lesson Name")]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [StringLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        public int CourseId { get; set; }

        public bool? IsViewedByTeacher { get; set; }

        public ICollection<LearningMaterialViewModel>? LearningMaterials { get; set; }
    }
}
