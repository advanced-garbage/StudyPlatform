using Microsoft.EntityFrameworkCore;
using StudyPlatform.Data;
using StudyPlatform.Services.Users.Interfaces;
using StudyPlatform.Web.View.Models.Student;
using StudyPlatform.Web.View.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudyPlatform.Common.ModelValidationConstants;

namespace StudyPlatform.Services.Users
{
    public class UserService : IUserService
    {
        private readonly StudyPlatformDbContext _db;

        public UserService(StudyPlatformDbContext db)
        {
            this._db = db;
        }
        public async Task<bool> AnyById(Guid id)
        {
            bool userExists
                = await this._db
                .Users
                .AnyAsync(u => u.Id.Equals(id));

            return userExists;
        }

        public async Task<bool> AnyByUserName(string userName)
        {
            bool userExists
                = await this._db
                .Users
                .AnyAsync(u => u.UserName.Equals(userName));

            return userExists;
        }

        public async Task<UserViewModel> GetUserByIdAsync(Guid id)
        {
            UserViewModel userModel
                = await this._db
                .Users
                .Where(u => u.Id.Equals(id))
                .Select(u => new UserViewModel()
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    MiddleName = u.MiddleName,
                    LastName = u.LastName,
                    Age = u.Age,
                    Role = "Student"
                })
                .FirstOrDefaultAsync();

            return userModel;
        }
    }
}
