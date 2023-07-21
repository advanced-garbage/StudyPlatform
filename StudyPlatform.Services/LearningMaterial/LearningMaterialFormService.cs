using StudyPlatform.Data;
using StudyPlatform.Services.LearningMaterial.Interfaces;
using StudyPlatform.Web.View.Models.LearningMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Services.LearningMaterial
{
    public class LearningMaterialFormService : ILearningMaterialFormService
    {
        private readonly StudyPlatformDbContext _db;
        public LearningMaterialFormService(StudyPlatformDbContext db)
        {
            this._db = db;
        }
        public async Task AddLessonAsync(UploadLearningMaterialFormModel model)
        {
            Data.Models.LearningMaterial lmObj
                = new Data.Models.LearningMaterial()
                {
                    FileName = model.File.FileName,
                    LessonId = model.LessonId,
                    LearningMaterialName = model.LearningMaterialName
                };

            await this._db.LearningMaterials.AddAsync(lmObj);
            await this._db.SaveChangesAsync();
        }
    }
}
