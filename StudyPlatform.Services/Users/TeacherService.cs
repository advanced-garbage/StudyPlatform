using Microsoft.EntityFrameworkCore;
using StudyPlatform.Data;
using StudyPlatform.Services.LearningMaterial;
using StudyPlatform.Services.Users.Interfaces;
using StudyPlatform.Web.View.Models.Teacher;
using System.Data;
using static StudyPlatform.Common.ViewModelConstants.Account;
namespace StudyPlatform.Services.Users
{
    /// <summary>
    /// Teacher Dependency mainly used for reading operations.
    /// </summary>
    public class TeacherService : ITeacherService
    {
        /// <summary>
        /// DbContext dependency.
        /// </summary>
        private readonly StudyPlatformDbContext _db;

        public TeacherService(StudyPlatformDbContext db)
        {
            this._db = db;
        }

        public async Task<bool> AnyById(Guid id)
        {
            bool teacherExists
                = await this._db
                .Teachers
                .AnyAsync(u => u.Id.Equals(id));

            return teacherExists;
        }

        public async Task<ICollection<TeacherViewModel>> GetAllAsync()
        {
            ICollection<TeacherViewModel> teachers
                = await this._db
                .Users
                .Select(u => new TeacherViewModel()
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    MiddleName = u.MiddleName,
                    LastName = u.LastName,
                    Age = u.Age,
                    Role = TeacherRoleTitle
                })
                .ToListAsync();

            return teachers;
        }


        public async Task<TeacherViewModel> GetAsync(Guid id)
        {
            TeacherViewModel userModel
                = await this._db
                .Users
                .Where(u => u.Id.Equals(id))
                .Select(u => new TeacherViewModel()
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    MiddleName = u.MiddleName,
                    LastName = u.LastName,
                    Age = u.Age,
                    Role = TeacherRoleTitle
                })
                .FirstOrDefaultAsync();

            return userModel;
        }

        public ICollection<TeacherForLearningMaterialViewModel> GetByLessonId(int lessonId)
        {
            ICollection<Guid> teacherIds
                = this._db
                .TeacherLessons
                .Where(lm => lm.LessonId.Equals(lessonId))
                .Select(t => (Guid)t.TeacherId)
                .ToList();

            ICollection<TeacherForLearningMaterialViewModel> userModels
                = this._db
                .Users
                .Where(t => teacherIds.Contains(t.Id))
                .Select(t => new TeacherForLearningMaterialViewModel()
                {
                    Id = t.Id,
                    UserName = t.UserName,
                    FirstName = t.FirstName,
                    LastName = t.LastName
                })
                .ToList();

            return userModels;
        }
    }
}
