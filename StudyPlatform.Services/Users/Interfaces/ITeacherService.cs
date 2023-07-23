using StudyPlatform.Web.View.Models.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Services.Users.Interfaces
{
    public interface ITeacherService
    {
        /// <summary>
        /// Returns whether a teacher by this Guid exists in the Database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> AnyById(Guid id);

        /// <summary>
        /// Returns a TeacherViewModel with the given Guid.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TeacherViewModel> GetAsync(Guid id);

        /// <summary>
        /// Returns a collection of TeacherViewModels.
        /// </summary>
        /// <returns></returns>
        Task<ICollection<TeacherViewModel>> GetAllAsync();

        /// <summary>
        /// Returns a collection of TeacherViewModel entities that share this learningMaterialId in the TeacherLessons Table.
        /// </summary>
        /// <param name="learningMaterial"></param>
        /// <returns></returns>
        ICollection<TeacherForLearningMaterialViewModel> GetByLessonId(int lessonId);
       
    }
}
