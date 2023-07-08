using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.Category;

namespace StudyPlatform.Data.Models
{
    // class for categorizing courses
    public class Category
    {
        public Category() {
            this.Courses = new List<Course>();
        }   
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        public ICollection<Course> Courses { get; set; }
    }
}
