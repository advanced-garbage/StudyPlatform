using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyPlatform.Data.Models;
namespace StudyPlatform.Data.Configuration
{
    public class TeacherLessonConfiguration : IEntityTypeConfiguration<TeacherLesson>
    {
        public void Configure(EntityTypeBuilder<TeacherLesson> tlBuilder)
        {
            
            tlBuilder
                .HasKey(tl => new { tl.LessonId, tl.TeacherId });
    
        }
    }
}
