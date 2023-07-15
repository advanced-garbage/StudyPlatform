using Microsoft.EntityFrameworkCore;
using StudyPlatform.Data;
using StudyPlatform.Services.Category.Interfaces;
using StudyPlatform.Web.View.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Services.Category
{
    public class CategoryViewFormService : ICategoryViewFormService
    {
        private readonly StudyPlatformDbContext _db;

        public CategoryViewFormService(StudyPlatformDbContext db)
        {
            this._db = db;
        }
        public async Task AddAsync(CategoryViewFormModel model)
        {
            Data.Models.Category categoryObj
                = new Data.Models.Category
                {
                    Name = model.Name
                };

            await this._db.AddAsync(categoryObj);
            await this._db.SaveChangesAsync();
        }

        public async Task EditAsync(CategoryViewFormModel model)
        {
            Data.Models.Category categoryObj
                = await this._db
                .Categories
                .Where(c => c.Id.Equals(model.Id))
                .FirstOrDefaultAsync();

            categoryObj.Name = model.Name;
            await this._db.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            if (!await AnyCategoryByIdAsync(id))
            {
                throw new InvalidOperationException($"Category with this id ({id}) was not found.");
            }

            Data.Models.Category categoryObj
                = await this._db
                .Categories
                .FirstOrDefaultAsync(c => c.Id.Equals(id));

            this._db.Categories.Remove(categoryObj);
            await this._db.SaveChangesAsync();
        }

        private async Task<bool> AnyCategoryByIdAsync(int id)
        {
            bool categoryExists
                = await _db
                .Categories
                .AnyAsync(c => c.Id.Equals(id));

            return categoryExists;
        }
    }
}
