using Microsoft.EntityFrameworkCore;
using StudyPlatform.Data;
using StudyPlatform.Data.Models;
using StudyPlatform.Services.LearningMaterial.Interfaces;
using StudyPlatform.Web.View.Models.Lesson;

namespace StudyPlatform.Services.LearningMaterial
{
    public class LearningMaterialService : ILearningMaterialService
    {
        private readonly StudyPlatformDbContext _db;
        public LearningMaterialService(StudyPlatformDbContext db)
        {
            this._db = db;
        }

        public async Task<bool> AnyByIdAsync(int id)
        {
            bool lmExists 
                = await this._db
                .LearningMaterials
                .AnyAsync(lm => lm.Id.Equals(id));

            return lmExists;
        }

        public async Task<bool> AnyByNameAsync(string name)
        {
            bool lmExists
                = await this._db
                .LearningMaterials
                .AnyAsync(lm => lm.LearningMaterialName.Equals(name));

            return lmExists;
        }
    }
}
