using StudyPlatform.Web.View.Models.Course.Interfaces;
using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.Course;
namespace StudyPlatform.Web.View.Models.Course
{
    public class AccountCourseViewModel : ICourseLink
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        public int CategoryId { get; set; }
    }
}
