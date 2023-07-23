using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyPlatform.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Data.Configuration
{
    public class LearningMaterialConfiguration : IEntityTypeConfiguration<LearningMaterial>
    {
        public void Configure(EntityTypeBuilder<LearningMaterial> lmBuilder)
        {
            lmBuilder
                .HasOne(lm => lm.Lesson)
                .WithMany(l => l.LearningMaterials)
                .HasForeignKey(lm => lm.LessonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
