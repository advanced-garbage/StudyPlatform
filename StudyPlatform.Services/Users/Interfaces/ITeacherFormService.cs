
using StudyPlatform.Data.Models;

namespace StudyPlatform.Services.Users.Interfaces
{
    public interface ITeacherFormService
    {
        Task AddTeacherAsync(Guid id);

        Task UpdateRoleToTeacherAsync(Guid id, ApplicationUser user);
    }
}
