using Microsoft.EntityFrameworkCore;
using StudyPlatform.Data;
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
    }
}
