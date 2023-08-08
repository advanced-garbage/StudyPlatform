using StudyPlatform.Web.View.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Services.Users.Interfaces
{
    public interface IUserService
    {
        public Task<UserAccountViewModel> GetUserByIdAsync(Guid id);

        public Task<bool> AnyByUserName(string userName);

        public Task<bool> AnyById(Guid id);

        Task<Guid> GetGuidByUsernameAsync(string username);
    }
}
