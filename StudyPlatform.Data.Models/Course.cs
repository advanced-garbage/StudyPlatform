using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudyPlatform.Common.ModelValidationConstants.Course;

namespace StudyPlatform.Data.Models
{
    // class for categorizing lesson(s)
    public class Course
    {
        public Course() {
            this.Lessons = new List<Lesson>();
        }
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [StringLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Lesson> Lessons { get; set; }    
    }
}
