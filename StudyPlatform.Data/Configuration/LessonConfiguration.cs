using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyPlatform.Data.Models;

namespace StudyPlatform.Data.Configuration
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> lessonBuilder)
        {
            lessonBuilder
                .HasOne(l => l.Course)
                .WithMany(c => c.Lessons)  
                .HasForeignKey(l => l.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
