
namespace StudyPlatform.Mapper.DTO
{
    public class CategoryDTO
    {
        public CategoryDTO()
        {
            this.Courses = new List<CourseDTO>();
        }

        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public bool IsViewedByTeacher { get; set; }
        public ICollection<CourseDTO> Courses { get; set; }
    }
}
