using StudyPlatform.Data;
using StudyPlatform.Services.Users.Interfaces;

namespace StudyPlatform.Services.Users
{
    public class TeacherFormService : ITeacherFormService
    {
        /// <summary>
        /// DbContext dependency.
        /// </summary>
        private readonly StudyPlatformDbContext _db;

        public TeacherFormService(StudyPlatformDbContext db)
        {
            this._db = db;
        }

        public async Task AddTeacher(Guid id)
        {
            Data.Models.Teacher teacherObj
                = new Data.Models.Teacher()
                {
                    Id = id
                };

            await this._db.Teachers.AddAsync(teacherObj);
            await this._db.SaveChangesAsync();
        }
    }
}
