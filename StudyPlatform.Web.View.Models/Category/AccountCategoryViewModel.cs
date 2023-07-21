using StudyPlatform.Web.View.Models.Category.Interfaces;
using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.Course;

namespace StudyPlatform.Web.View.Models.Category
{
    public class AccountCategoryViewModel : ICategoryLink
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }
    }
}
