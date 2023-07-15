using Microsoft.EntityFrameworkCore;
using StudyPlatform.Data;
using StudyPlatform.Services.Lesson.Interfaces;
using StudyPlatform.Web.View.Models.Lesson;

namespace StudyPlatform.Services.Lesson
{
    public class LessonViewService : ILessonViewService
    {
        private readonly StudyPlatformDbContext _db;
        public LessonViewService(StudyPlatformDbContext db)
        {
            this._db = db;
        }
        public async Task<bool> AnyByIdAsync(int id)
        {
            bool lessonExists
                = await this._db
                .Lessons
                .AnyAsync(l => l.Id.Equals(id));

            return lessonExists;
        }

        public async Task<bool> AnyByNameAsync(string name)
        {
            bool lessonExists
                = await this._db
                .Lessons
                .AnyAsync(l => l.Name.Equals(name));

            return lessonExists;
        }

        public async Task<ICollection<LessonViewModel>> GetAllLessonsByCourseIdAsync(int courseId)
        {
            ICollection<LessonViewModel> lessonModels =
                await this._db
                .Lessons
                .Where(c => c.CourseId.Equals(courseId))
                .Select(l => new LessonViewModel()
                {
                    Id = l.Id,
                    CourseId = l.CourseId,
                    Name = l.Name,
                    Description = l.Description
                })
                .ToListAsync();

            return lessonModels;
        }

        public async Task<int> GetCourseIdByLessonId(int id)
        {
            int courseId = 
                await this._db
                .Lessons
                .Where(c => c.Id.Equals(id))
                .Select(c => c.CourseId)
                .FirstAsync();

            return courseId;
        }

        public async Task<LessonViewModel> GetLessonByIdAsync(int id)
        {
            LessonViewModel lessonModel
                = await this._db
                .Lessons
                .Where(l => l.Id.Equals(id))
                .Select(l => new LessonViewModel()
                {
                    Id = l.Id,
                    Name = l.Name,
                    Description = l.Description,
                    CourseId = l.CourseId
                })
                .FirstOrDefaultAsync();

            return lessonModel;
        }

        public async Task<string> GetNameByIdAsync(int id)
        {
            string lessonName =
                await this._db
                .Lessons
                .Where(c => c.Id.Equals(id))
                .Select(c => c.Name)
                .Take(1)
                .FirstOrDefaultAsync();

            return lessonName;
        }
    }
}
