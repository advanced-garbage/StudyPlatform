using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyPlatform.Data.Models;

namespace StudyPlatform.Data.Configuration
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> courseBuilder)
        {
            courseBuilder
                .HasOne(c => c.Category)
                .WithMany(c => c.Courses)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            courseBuilder
                .HasData(SeedCourse());
        }

        private ICollection<Course> SeedCourse()
        {
            ICollection<Course> result = new HashSet<Course>();

            result.Add(new Course()
            {
                Id = 1,
                Name = "Introduction to Programming (C#)",
                CategoryId = 1,
            });

            result.Add(new Course()
            {
                Id = 2,
                Name = "Object-Oriented Programming (C#)",
                CategoryId = 1,
            });

            return result;
        }
    }
}
