using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyPlatform.Data.Models;
namespace StudyPlatform.Data.Configuration
{
    public class TeacherLessonConfiguration : IEntityTypeConfiguration<TeacherLearningMaterial>
    {
        public void Configure(EntityTypeBuilder<TeacherLearningMaterial> tlBuilder)
        {
            tlBuilder
                .HasKey(tl => new { tl.LessonId, tl.TeacherId });
        }
    }
}
