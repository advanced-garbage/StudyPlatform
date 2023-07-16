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
        public async Task AddAsync(Guid teacherId, int lmId)
        {
            Data.Models.TeacherLearningMaterial tl
                = new Data.Models.TeacherLearningMaterial()
                {
                    TeacherId = teacherId,
                    LessonId = lmId
                };

            await this._db.TeacherLessons.AddAsync(tl);
            await this._db.SaveChangesAsync();
        }
    }
}
