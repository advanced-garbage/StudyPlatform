using StudyPlatform.Data;
using StudyPlatform.Data.Models;
using StudyPlatform.Services.LearningMaterial.Interfaces;

namespace StudyPlatform.Services.LearningMaterial
{
    public class LearningMaterialService : ILearningMaterialService
    {
        private readonly StudyPlatformDbContext _db;
        public LearningMaterialService(StudyPlatformDbContext db)
        {
            this._db = db;
        }
        public async Task AddLessonAsync(string fileName)
        {
            Data.Models.LearningMaterial lmObj
                = new Data.Models.LearningMaterial()
                {
                    FileName = fileName,
                };

            await this._db.LearningMaterials.AddAsync(lmObj);
            await this._db.SaveChangesAsync();
        }
    }
}
