using Microsoft.EntityFrameworkCore;
using StudyPlatform.Data;
using StudyPlatform.Services.Course.Interfaces;
using StudyPlatform.Web.View.Models.Course;
using StudyPlatform.Web.View.Models.Lesson;

namespace StudyPlatform.Services.Course
{
    public class CourseViewService : ICourseViewService
    {
        private readonly StudyPlatformDbContext _db;

        public CourseViewService(StudyPlatformDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(CourseViewFormModel model)
        {
            Data.Models.Course courseObj
                = new Data.Models.Course()
                {
                    Name = model.Name,
                    CategoryId = model.CategoryId,
                    Description = model.Description,
                };

            await this._db.Courses.AddAsync(courseObj);
            await this._db.SaveChangesAsync();
        }

        public async Task AssignToNewCategoryAsync(int courseId, int categoryId)
        {
            Data.Models.Course courseObj
                = await this._db
                .Courses
                .Where(c => c.Id.Equals(courseId))
                .FirstOrDefaultAsync();

            courseObj.CategoryId = categoryId;
            await this._db.SaveChangesAsync();
        }

        public async Task EditAsync(CourseViewFormModel model)
        {
            Data.Models.Course courseObj
                = await this._db
                .Courses
                .Where(c => c.Id.Equals(model.Id))
                .FirstOrDefaultAsync();

            courseObj.Name = model.Name;
            courseObj.Description = model.Description;
            courseObj.CategoryId = model.CategoryId;
            await this._db.SaveChangesAsync();
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

        public async Task<CourseViewModel> GetCourseById(int id)
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
                    Lessons = c.Lessons.Select(l => new LearningMaterialViewModel()
                    {
                        Id = l.Id,
                        FileName = l.Name,
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
                throw new InvalidOperationException($"course by this id ({id}) was not found");
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
                throw new InvalidOperationException($"course by this id ({courseId}) was not found");
            }

            string Name =
                await this._db
                .Courses
                .Where(c => c.Id.Equals(courseId))
                .Select(c => c.Name)
                .Take(1)
                .FirstOrDefaultAsync();

            return Name;
        }

        public async Task RemoveAsync(int id)
        {
            if (!await AnyByIdAsync(id))
            {
                throw new InvalidOperationException($"A course by this id ({id}) was not found");
            }

            Data.Models.Course courseObj
                = await this._db
                .Courses
                .Where(c => c.Equals(id))
                .FirstOrDefaultAsync();

            this._db.Courses.Remove(courseObj);
            await this._db.SaveChangesAsync();
        }

        private async Task<bool> AnyByIdAsync(int id)
        {
            bool courseExists = await this._db.Courses.AnyAsync(c => c.Id.Equals(id));

            return courseExists;
        }
    }
}
