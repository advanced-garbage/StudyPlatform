using Microsoft.EntityFrameworkCore;
using StudyPlatform.Data;
using StudyPlatform.Services.Course.Interfaces;
using StudyPlatform.Web.View.Models.Course;
using static StudyPlatform.Common.ErrorMessages.Course;


namespace StudyPlatform.Services.Course
{
    public class CourseViewFormService : ICourseViewFormService
    {
        private readonly StudyPlatformDbContext _db;

        public CourseViewFormService(StudyPlatformDbContext db)
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

        public async Task RemoveAsync(int id)
        {
            if (!await AnyByIdAsync(id))
            {
                throw new InvalidOperationException(CourseIdNotFound);
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
