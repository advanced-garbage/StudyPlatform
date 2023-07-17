namespace StudyPlatform.Services.TeacherLesson.Intefaces
{
    public interface ITeacherLearningMaterialService
    {
        Task AddAsync(Guid teacherId, int lmId);

        Task<Guid> GetTeacherIdByLmIdAsync(int lmId);

        Task<int> GetLmIdByTeacherGuidAsync(Guid teacherId);
    }
}
