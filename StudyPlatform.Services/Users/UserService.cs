using Microsoft.EntityFrameworkCore;
using StudyPlatform.Data;
using StudyPlatform.Services.Users.Interfaces;
using StudyPlatform.Web.View.Models.User;
using static StudyPlatform.Common.ViewModelConstants.Account;



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

        public async Task<UserAccountViewModel> GetUserByIdAsync(Guid id)
        {
            UserAccountViewModel userModel
                = await this._db
                .Users
                .Where(u => u.Id.Equals(id))
                .Select(u => new UserAccountViewModel()
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    MiddleName = u.MiddleName,
                    LastName = u.LastName,
                    Age = u.Age
                })
                .FirstOrDefaultAsync();

            return userModel;
        }
    }
}
