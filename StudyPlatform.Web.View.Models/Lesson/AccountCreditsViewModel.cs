using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.Lesson;
using static StudyPlatform.Common.ModelValidationConstants.Course;
using static StudyPlatform.Common.ModelValidationConstants.Category;
using StudyPlatform.Web.View.Models.Course;
using StudyPlatform.Web.View.Models.Category;

namespace StudyPlatform.Web.View.Models.Lesson
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountCreditsViewModel
    {
        public AccountCreditsViewModel() {
            this.AccountLessons = new List<AccountLessonViewModel>();
            this.AccountCourses = new List<AccountCourseViewModel>();
            this.AccountCategories = new List<AccountCategoryViewModel>();
        }
        public int LearningMaterialId { get; set; }
        public ICollection<AccountLessonViewModel> AccountLessons;
        public ICollection<AccountCourseViewModel> AccountCourses;
        public ICollection<AccountCategoryViewModel> AccountCategories;
    }
}
