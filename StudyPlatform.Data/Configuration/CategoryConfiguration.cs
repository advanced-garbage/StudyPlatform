using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyPlatform.Data.Models;

namespace StudyPlatform.Data.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> categoryBuilder)
        {
            categoryBuilder
                .HasData(new Category() { Id = 1, Name = "Computer Science" });
        }

    }
}
