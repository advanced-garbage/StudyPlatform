using StudyPlatform.Web.View.Models.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Services.Lesson.Interfaces
{
    public interface ILessonFormService
    {
        public Task<LessonViewFormModel> GetFormByIdAsync(int id);
        public Task AddAsync(LessonViewFormModel model);

        public Task RemoveAsync(int lessonId);

        public Task EditAsync(LessonViewFormModel model);
    }
}
