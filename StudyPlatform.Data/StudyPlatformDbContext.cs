using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyPlatform.Data.Models;
using System.Reflection;

namespace StudyPlatform.Data
{
    public class StudyPlatformDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public StudyPlatformDbContext(DbContextOptions<StudyPlatformDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Teacher> Teachers { get; set; }        
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LearningMaterial> LearningMaterials { get; set; }
        public DbSet<TeacherLesson> TeacherLessons { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(StudyPlatformDbContext)));
        }
    }
}