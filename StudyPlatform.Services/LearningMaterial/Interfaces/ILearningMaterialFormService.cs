using StudyPlatform.Web.View.Models.LearningMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Services.LearningMaterial.Interfaces
{
    public interface ILearningMaterialFormService
    {
        public Task AddLessonAsync(UploadLearningMaterialFormModel model);
    }
}
