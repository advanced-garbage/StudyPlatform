using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyPlatform.Data.Models;

namespace StudyPlatform.Data.Configuration
{
    public class StudentLessonConfiguration : IEntityTypeConfiguration<StudentLesson>
    {
        public void Configure(EntityTypeBuilder<StudentLesson> slBuilder)
        {
            slBuilder
                .HasKey(sl => new {sl.StudentId, sl.LessonId});
        }
    }
}
