namespace StudyPlatform.Services.Roles.Interfaces
{
    public interface IRoleService
    {
        Task<string> GetRoleNameAsync();

        Task<bool> IsTeacherRole();
    }
}
