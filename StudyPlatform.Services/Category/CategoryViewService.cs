using Microsoft.EntityFrameworkCore;
using StudyPlatform.Data;
using StudyPlatform.Services.Category.Interfaces;
using StudyPlatform.Web.View.Models.Category;
using StudyPlatform.Web.View.Models.Course;
using System.Reflection.Metadata.Ecma335;
using StudyPlatform.Data.Models;
using static StudyPlatform.Common.ModelValidationConstants;

namespace StudyPlatform.Services.Category
{
    public class CategoryViewService : ICategoryViewService
    {
        private readonly StudyPlatformDbContext _db;

        public CategoryViewService(StudyPlatformDbContext db)
        {
            _db = db;
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

        public async Task<bool> AnyByNameAsync(string name)
        {
            bool existsByName = 
                await this._db
                .Categories
                .AnyAsync(c => c.Name.Equals(name));

            return existsByName;
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

        public async Task<ICollection<CategoryViewModel>> GetAllCategoriesAsync()
        {
            ICollection<CategoryViewModel> categories
                = await _db
                .Categories
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return categories;
        }

        public async Task<CategoryViewModel> GetCategoryByIdAsync(int id)
        {
            if (await AnyByIdAsync(id))
            {
                CategoryViewModel? category
                    = await _db
                    .Categories
                    .Where(c => c.Id.Equals(id))
                    .Select(c => new CategoryViewModel()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Courses = c.Courses.Select(a => new CourseViewModel()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            CategoryId = a.CategoryId
                        })
                        .ToList()
                    })
                    .FirstOrDefaultAsync();

                return category;
            }

            throw new InvalidOperationException($"A Category by this id ({id}) was not found.");
        }

        public async Task<int> GetCategoryIdByCourseIdAsync(int courseId)
        {
            if (!await this._db.Courses.AnyAsync(c => c.Id.Equals(courseId)))
            {
                throw new InvalidOperationException($"The provided id ({courseId}) was not found!");
            }

            int categoryId
                = await this._db
                .Courses
                .Where(c => c.Id.Equals(courseId))
                .Select(c => c.CategoryId)
                .FirstOrDefaultAsync();

            return categoryId;
        }

        public async Task<CategoryViewFormModel> GetFormCategory(int id)
        {
            if (!await AnyByIdAsync(id))
            {
                throw new InvalidOperationException($"A Category by this id {id} was not found");
            }

            CategoryViewFormModel model
                = await this._db
                .Categories
                .Where(c => c.Id.Equals(id))
                .Select(c => new CategoryViewFormModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .FirstOrDefaultAsync();

            return model;
        }

        public async Task<string> GetNameByIdAsync(int categoryId)
        {
            if (!await AnyByIdAsync(categoryId))
            {
                throw new InvalidOperationException($"course by this id ({categoryId}) was not found");
            }

            string Name = 
                await this._db
                .Categories
                .Where(c => c.Id.Equals(categoryId))
                .Select(c => c.Name)
                .Take(1)
                .FirstOrDefaultAsync();

            return Name;
        }

        public async Task RemoveAsync(int id)
        {
            if (!await AnyByIdAsync(id)) 
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

        private async Task<bool> AnyByIdAsync(int id)
        {
            bool categoryExists
                = await _db
                .Categories
                .AnyAsync(c => c.Id.Equals(id));

            return categoryExists;
        }
    }
}
