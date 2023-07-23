using Microsoft.EntityFrameworkCore;
using StudyPlatform.Data;
using StudyPlatform.Data.Models;
using StudyPlatform.Services.Course.Interfaces;
using StudyPlatform.Services.Lesson.Interfaces;
using StudyPlatform.Web.View.Models.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudyPlatform.Common.ModelValidationConstants;
using Lesson = StudyPlatform.Data.Models.Lesson;

namespace StudyPlatform.Services.Lesson
{
    public class LessonFormService : ILessonFormService
    {
        private readonly StudyPlatformDbContext _db;
        private readonly ICourseViewService _courseViewService;
        public LessonFormService(
            StudyPlatformDbContext db,
            ICourseViewService courseViewService)
        {
            this._db = db;
            this._courseViewService = courseViewService;
        }
        public async Task AddAsync(LessonViewFormModel model)
        {
            Data.Models.Lesson lessonObj = new Data.Models.Lesson()
            {
                Name = model.Name,
                Description = model.Description,
                CourseId = model.CourseId
            };

            await this._db.Lessons.AddAsync(lessonObj);
            await this._db.SaveChangesAsync();
        }

        public async Task RemoveAsync(int lessonId)
        {
            Data.Models.Lesson lessonObj
                = await this._db
                .Lessons
                .Where(c => c.Id.Equals(lessonId))
                .FirstAsync();

            ICollection<Data.Models.TeacherLesson> lessons 
                = await this._db.TeacherLessons
                .Where(tl => tl.LessonId.Equals(lessonId))
                .ToListAsync();

            this._db.TeacherLessons.RemoveRange(lessons);
            await this._db.SaveChangesAsync();

            this._db.Lessons.Remove(lessonObj);
            await this._db.SaveChangesAsync();
        }

        public async Task EditAsync(LessonViewFormModel model)
        {
            Data.Models.Lesson lessonObj
            = await this._db
                .Lessons
                .Where(c => c.Id.Equals(model.Id))
                .FirstAsync();

            lessonObj.CourseId = model.CourseId;
            lessonObj.Name = model.Name;
            lessonObj.Description = model.Description;
            await this._db.SaveChangesAsync();
        }

        public async Task<LessonViewFormModel> GetFormByIdAsync(int id)
        {
            LessonViewFormModel model
                = await this._db
                .Lessons
                .Where(l => l.Id.Equals(id))
                .Select(l => new LessonViewFormModel()
                {
                    Id = l.Id,
                    Name = l.Name,
                    Description = l.Description,
                    CourseId = l.CourseId
                })
                .FirstOrDefaultAsync();
            model.Courses = await this._courseViewService.GetAllAsync();
            return model;
        }

    }
}
