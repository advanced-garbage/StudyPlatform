using Microsoft.EntityFrameworkCore;
using StudyPlatform.Data;
using StudyPlatform.Services.Category.Interfaces;
using StudyPlatform.Services.Course.Interfaces;
using StudyPlatform.Services.LearningMaterial.Interfaces;
using StudyPlatform.Services.Lesson.Interfaces;
using StudyPlatform.Web.View.Models.Lesson;
using StudyPlatform.Web.View.Models.Course;
using StudyPlatform.Web.View.Models.Category;

namespace StudyPlatform.Services.Lesson
{
    public class LessonViewService : ILessonViewService
    {
        private readonly StudyPlatformDbContext _db;
        private readonly ILearningMaterialService _learningMaterialService;
        private readonly ICourseViewService _courseViewService;
        private readonly ICategoryViewService _categoryViewService;
        public LessonViewService(
            StudyPlatformDbContext db,
            ILearningMaterialService learningMaterialService,
            ICourseViewService courseViewService,
            ICategoryViewService categoryViewService)
        {
            this._db = db;
            this._learningMaterialService = learningMaterialService;
            this._courseViewService = courseViewService;
            this._categoryViewService = categoryViewService;
        }
        public async Task<bool> AnyByIdAsync(int id)
        {
            bool lessonExists
                = await this._db
                .Lessons
                .AnyAsync(l => l.Id.Equals(id));

            return lessonExists;
        }

        public async Task<bool> AnyByNameAsync(string name)
        {
            bool lessonExists
                = await this._db
                .Lessons
                .AnyAsync(l => l.Name.Equals(name));

            return lessonExists;
        }

        public async Task<AccountCreditsViewModel> GetAccountCreditsAsync(Guid teacherId)
        {
            AccountCreditsViewModel model = new AccountCreditsViewModel();
            IList<int> lessonIds = new List<int>();
            IList<int> courseIds = new List<int>();
            IList<int> categoryIds = new List<int>();

            // take every learning material id that shares the passed teacherId
            IList<int> lmIds 
                = await this._db.TeacherLessons
                .Where(c => c.TeacherId.Equals(teacherId))
                .Select(lm => lm.LessonId)
                .ToListAsync();

            foreach (int lmId in lmIds)
            {
                // find lessonid through learning material ids 
                int lessonId = await GetLessonIdByLearningMaterialId(lmId);
                // find courseId through lessonId
                int courseId = await GetCourseIdByLessonId(lessonId);
                // find categoryId through courseId
                int categoryId = await this._courseViewService.GetCategoryIdByCourseIdAsync(courseId);

                if (!categoryIds.Contains(categoryId)) {
                    categoryIds.Add(categoryId);
                    string categoryName = await this._categoryViewService.GetNameByIdAsync(categoryId);
                    model.AccountCategories.Add(new AccountCategoryViewModel()
                    {
                        Id = categoryId,
                        Name = categoryName
                    });
                }
                if (!courseIds.Contains(courseId)) {   
                    courseIds.Add(courseId);
                    string courseName = await this._courseViewService.GetNameByIdAsync(courseId);
                    model.AccountCourses.Add(new AccountCourseViewModel()
                    {
                        Id = courseId,
                        Name = courseName,
                        CategoryId = categoryId
                    });
                }
                if (!lessonIds.Contains(lessonId)) {
                    lessonIds.Add(lessonId);
                    string lessonName = await this.GetNameByIdAsync(lessonId);
                    model.AccountLessons.Add(new AccountLessonViewModel()
                    {
                        Id = lessonId,
                        Name = lessonName,
                        CourseId = courseId
                    });
                }
            }
            return model;
        }

        public async Task<ICollection<LessonViewModel>> GetAllLessonsByCourseIdAsync(int courseId)
        {
            ICollection<LessonViewModel> lessonModels =
                await this._db
                .Lessons
                .Where(c => c.CourseId.Equals(courseId))
                .Select(l => new LessonViewModel()
                {
                    Id = l.Id,
                    CourseId = l.CourseId,
                    Name = l.Name,
                    Description = l.Description
                })
                .ToListAsync();

            return lessonModels;
        }

        public async Task<int> GetCourseIdByLessonId(int id)
        {
            int courseId = 
                await this._db
                .Lessons
                .Where(c => c.Id.Equals(id))
                .Select(c => c.CourseId)
                .FirstAsync();

            return courseId;
        }

        public async Task<LessonViewModel> GetLessonByIdAsync(int id)
        {
            LessonViewModel lessonModel
                = await this._db
                .Lessons
                .Where(l => l.Id.Equals(id))
                .Select(l => new LessonViewModel()
                {
                    Id = l.Id,
                    Name = l.Name,
                    Description = l.Description,
                    CourseId = l.CourseId,
                    LearningMaterials = this._learningMaterialService.GetAllModelsByLesson(l.Id)
                })
                .FirstOrDefaultAsync();

            return lessonModel;
        }

        public async Task<int> GetLessonIdByLearningMaterialId(int lmId)
        {
            int lessonId
                = await this._db
                .LearningMaterials
                .Where(lm => lm.Id.Equals(lmId))
                .Select(lm => lm.LessonId)
                .FirstAsync();

            return lessonId;
        }

        public async Task<string> GetNameByIdAsync(int id)
        {
            string lessonName =
                await this._db
                .Lessons
                .Where(c => c.Id.Equals(id))
                .Select(c => c.Name)
                .Take(1)
                .FirstOrDefaultAsync();

            return lessonName;
        }
    }
}
