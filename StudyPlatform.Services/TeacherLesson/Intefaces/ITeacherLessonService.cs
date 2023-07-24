using StudyPlatform.Web.View.Models.Teacher;

namespace StudyPlatform.Services.TeacherLesson.Intefaces
{
    public interface ITeacherLessonService
    {
        Task AddAsync(Guid teacherId, int lessonId);

        Task<Guid> GetTeacherIdByLessonIdAsync(int lessonId);

        Task<int> GetLessonIdByTeacherGuidAsync(Guid teacherId);

        Task<ICollection<TeacherForLessonModel>> GetTeachersForLessonAsync(int lessonId);

        Task<bool> AnyTeacherByLessonId(int lessonId);
    }
}
