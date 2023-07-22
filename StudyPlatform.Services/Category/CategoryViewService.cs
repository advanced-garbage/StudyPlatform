using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyPlatform.Data;
using StudyPlatform.Services.Category.Interfaces;
using StudyPlatform.Services.Users.Interfaces;
using StudyPlatform.Web.View.Models.Category;
using StudyPlatform.Web.View.Models.Course;
using static StudyPlatform.Common.ErrorMessages.Category;
using static StudyPlatform.Common.ErrorMessages.Course;

namespace StudyPlatform.Services.Category
{
    public class CategoryViewService : ICategoryViewService
    {
        private readonly StudyPlatformDbContext _db;
        private readonly IMapper _mapper;
        public CategoryViewService(
            StudyPlatformDbContext db,
            IMapper mapper)
        {
            this._db = db;
            this._mapper = mapper;
        }

        public async Task<bool> AnyByIdAsync(int id)
        {
            bool categoryExists = 
                await this._db
                .Categories
                .AnyAsync(c => c.Id.Equals(id));

            return categoryExists;
        }

        public async Task<bool> AnyByNameAsync(string name)
        {
            bool existsByName = 
                await this._db
                .Categories
                .AnyAsync(c => c.Name.Equals(name));

            return existsByName;
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
                .OrderBy(c => c.Name)
                .ToListAsync();
            
            return categories;
        }

        public async Task<AllCategoriesViewModel> GetCategoriesForAllPageAsync()
        {
            AllCategoriesViewModel model = new AllCategoriesViewModel();
            model.Categories
                = await _db
                .Categories
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();
            model.IsViewedByTeacher = false;
            return model;
        }

        public async Task<CategoryViewModel> GetCategoryByIdAsync(int id)
        {
            if (await AnyCategoryByIdAsync(id))
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
                throw new InvalidOperationException(CourseIdNotFound);
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
            if (!await AnyCategoryByIdAsync(id))
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
            if (!await AnyCategoryByIdAsync(categoryId))
            {
                throw new InvalidOperationException(CategoryIdNotFound);
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
