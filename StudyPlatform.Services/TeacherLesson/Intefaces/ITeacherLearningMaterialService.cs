namespace StudyPlatform.Services.TeacherLesson.Intefaces
{
    public interface ITeacherLearningMaterialService
    {
        Task AddAsync(Guid teacherId, int lessonId);

        Task<Guid> GetTeacherIdByLessonIdAsync(int lessonId);

        Task<int> GetLessonIdByTeacherGuidAsync(Guid teacherId);
    }
}
