using Microsoft.EntityFrameworkCore;
using StudyPlatform.Data;
using StudyPlatform.Services.TeacherLesson.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Services.TeacherLesson
{
    public class TeacherLearningMaterialService : ITeacherLearningMaterialService
    {
        /// <summary>
        /// DbContext dependency.
        /// </summary>
        private readonly StudyPlatformDbContext _db;

        public TeacherLearningMaterialService(StudyPlatformDbContext db)
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
    }
}
