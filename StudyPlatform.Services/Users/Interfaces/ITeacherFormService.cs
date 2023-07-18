
namespace StudyPlatform.Services.Users.Interfaces
{
    public interface ITeacherFormService
    {
        Task AddTeacher(Guid id);

        Task UpdateRoleToTeacher(Guid id);
    }
}
