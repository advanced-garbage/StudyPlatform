using StudyPlatform.Web.View.Models.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Services.Lesson.Interfaces
{
    public interface ILessonViewService
    {
        public Task<ICollection<LessonViewModel>> GetAllLessonsByCourseIdAsync(int courseId);

        public Task<LessonViewModel> GetLessonByIdAsync(int id);

        public Task<int> GetCourseIdByLessonId(int id);

        public Task<bool> AnyByIdAsync(int id);

        public Task<bool> AnyByNameAsync(string name);

        public Task<string> GetNameByIdAsync(int id);

        public Task<AccountCreditsViewModel> GetAccountCreditsAsync(Guid teacherId);

        public Task<int> GetLessonIdByLearningMaterialId(int lmId);
    }
}
