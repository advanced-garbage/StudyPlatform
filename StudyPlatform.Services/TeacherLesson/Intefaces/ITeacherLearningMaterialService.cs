namespace StudyPlatform.Services.TeacherLesson.Intefaces
{
    public interface ITeacherLearningMaterialService
    {
        Task AddAsync(Guid teacherId, int lmId);
    }
}
