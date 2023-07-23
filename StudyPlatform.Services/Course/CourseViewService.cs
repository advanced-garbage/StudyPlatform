using Microsoft.EntityFrameworkCore;
using StudyPlatform.Data;
using StudyPlatform.Services.Course.Interfaces;
using StudyPlatform.Web.View.Models.Course;
using StudyPlatform.Web.View.Models.Lesson;
using static StudyPlatform.Common.ErrorMessages.Course;

namespace StudyPlatform.Services.Course
{
    public class CourseViewService : ICourseViewService
    {
        private readonly StudyPlatformDbContext _db;

        public CourseViewService(StudyPlatformDbContext db)
        {
            _db = db;
        }

        public async Task<int> GetCategoryIdByCourseIdAsync(int courseId)
        {
            int categoryId
                = (int)await this._db
                .Courses
                .Where(c => c.Id.Equals(courseId))
                .Select(c => c.CategoryId)
                .FirstOrDefaultAsync();

            return categoryId;
        }

        public async Task<CourseViewModel> GetById(int id)
        {
            CourseViewModel? course =
                await _db
                .Courses
                .Where(c => c.Id.Equals(id))
                .Select(c => new CourseViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Lessons = c.Lessons.Select(l => new LessonViewModel()
                    {
                        Id = l.Id,
                        Name = l.Name,
                        CourseId = l.CourseId
                    })
                    .ToList()
                })
                .FirstOrDefaultAsync();


            return course;
        }

        public async Task<CourseViewFormModel> GetFormCourseAsync(int id)
        {
            if (!await AnyByIdAsync(id))
            {
                throw new InvalidOperationException(CourseIdNotFound);
            }

            CourseViewFormModel courseModel
                = await this._db
                .Courses
                .Where(c => c.Id.Equals(id))
                .Select(c => new CourseViewFormModel()
                {
                    Name = c.Name,
                    Description = c.Description,
                    CategoryId = c.CategoryId
                })
                .FirstOrDefaultAsync();

            return courseModel;
        }

        public async Task<string> GetNameByIdAsync(int courseId)
        {
            if (!await AnyByIdAsync(courseId))
            {
                throw new InvalidOperationException(CourseIdNotFound);
            }

            string courseName =
                await this._db
                .Courses
                .Where(c => c.Id.Equals(courseId))
                .Select(c => c.Name)
                .Take(1)
                .FirstOrDefaultAsync();

            return courseName;
        }

        private async Task<bool> AnyByIdAsync(int id)
        {
            bool courseExists = await this._db.Courses.AnyAsync(c => c.Id.Equals(id));

            return courseExists;
        }

        public async Task<ICollection<CourseListViewModel>> GetAllAsync()
        {
            ICollection<CourseListViewModel> courses 
                = await this._db
                .Courses
                .Select(c => new CourseListViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return courses;
        }

        public async Task<int> GetIdAsync(int id)
        {
            int courseId
                = await this._db
                .Courses
                .Where(c => c.Id.Equals(id))
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            return courseId;
        }

        public async Task<int> GetIdByNameAsync(string courseName)
        {
            int courseId
                = await this._db
                .Courses
                .Where(c => c.Name.Equals(courseName))
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            return courseId;
        }
    }
}
