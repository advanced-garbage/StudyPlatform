using Microsoft.EntityFrameworkCore;
using StudyPlatform.Data;
using StudyPlatform.Services.Users.Interfaces;
using StudyPlatform.Web.View.Models.Student;

namespace StudyPlatform.Services.Users
{
    public class StudentService : IStudentService   
    {
        private readonly StudyPlatformDbContext _db;

        public StudentService(StudyPlatformDbContext db)
        {
            this._db = db;
        }

        public async Task<StudentViewModel> GetStudentAsync(string userId)
        {
            Guid guidUserId = new Guid(userId);
            var studentModel
                = await this._db
                .Users
                .Where(s => s.Id.Equals(guidUserId))
                .Select(s => new StudentViewModel()
                {
                    UserName = s.UserName,
                    Email = s.Email,
                    FirstName = s.FirstName,
                    MiddleName = s.MiddleName,
                    LastName = s.LastName,
                    Age = s.Age
                })
                .FirstOrDefaultAsync();

            return studentModel;
        }
    }
}
