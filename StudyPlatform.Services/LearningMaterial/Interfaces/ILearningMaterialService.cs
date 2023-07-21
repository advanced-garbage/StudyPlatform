using StudyPlatform.Web.View.Models.LearningMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Services.LearningMaterial.Interfaces
{
    public interface ILearningMaterialService
    {
        Task<bool> AnyByNameAsync(string name);

        Task<bool> AnyByIdAsync(int id);

        Task<int> GetIdByNameAsync(string lmName);
        Task<LearningMaterialViewModel> GetViewModelAsync(int lmId);

        Task<ICollection<LearningMaterialViewModel>> GetAllModelsByLessonAsync(int lessonId);

        ICollection<LearningMaterialViewModel> GetAllModelsByLesson(int lessonId);

    }
}
