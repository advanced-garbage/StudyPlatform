using Microsoft.EntityFrameworkCore;
using StudyPlatform.Data;
using StudyPlatform.Services.TeacherLesson.Intefaces;
using StudyPlatform.Web.View.Models.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Services.TeacherLesson
{
    public class TeacherLessonService : ITeacherLessonService
    {
        /// <summary>
        /// DbContext dependency.
        /// </summary>
        private readonly StudyPlatformDbContext _db;

        public TeacherLessonService(StudyPlatformDbContext db)
        {
            this._db = db;
        }
        public async Task AddAsync(Guid teacherId, int lessonId)
        {
            Data.Models.TeacherLesson tl
                = new Data.Models.TeacherLesson()
                {
                    TeacherId = teacherId,
                    LessonId = lessonId
                };

            await this._db.TeacherLessons.AddAsync(tl);
            await this._db.SaveChangesAsync();
        }

        public async Task<bool> TeacherLessonAlreadyExists(int lessonId, Guid teacherId)
        {
            bool teacherExists 
                = await this._db
                .TeacherLessons
                .AnyAsync(c => c.LessonId.Equals(lessonId) && c.TeacherId.Equals(teacherId));  

            return teacherExists;
        }

        public async Task<int> GetLessonIdByTeacherGuidAsync(Guid teacherId)
        {
            int lessonId
                = await this._db
                .TeacherLessons
                .Where(t => t.TeacherId.Equals(teacherId))
                .Select(lm => lm.LessonId)
                .FirstAsync();

            return lessonId;
        }

        public async Task<Guid> GetTeacherIdByLessonIdAsync(int lessonId)
        {
            Guid teacherGuid 
                = await this._db
                .TeacherLessons
                .Where(lm => lm.LessonId.Equals(lessonId))
                .Select(t => t.TeacherId)
                .FirstAsync();

            return teacherGuid;
        }

        public async Task<ICollection<TeacherForLessonModel>> GetTeachersForLessonAsync(int lessonId)
        {
            ICollection<Guid> teacherIds
                = await this._db
                .TeacherLessons
                .Where(tl => tl.LessonId.Equals(lessonId))
                .Select(tl => (Guid)tl.TeacherId)
                .ToListAsync();

            ICollection<TeacherForLessonModel> teacherModels
                = await this._db
                .Users
                .Where(t => teacherIds.Contains(t.Id))
                .Select(t => new TeacherForLessonModel()
                {
                    Id = t.Id,
                    UserName = t.UserName,
                    FirstName = t.FirstName,
                    MiddleName = t.MiddleName,
                    LastName = t.LastName
                })
                .OrderByDescending(t => t.FirstName)
                .ThenBy(t => t.LastName)
                .ThenBy(t => t.UserName)
                .ToListAsync();

            return teacherModels;
        }
    }
}
