using StudyPlatform.Web.View.Models.Category.Interfaces;
using StudyPlatform.Web.View.Models.Course.Interfaces;
using StudyPlatform.Web.View.Models.LearningMaterial.Interfaces;
using StudyPlatform.Web.View.Models.Lesson.Interfaces;

namespace StudyPlatform.Infrastructure.Infrastructure
{
    public static class LearningMaterialExtensions
    {
        public static string GetNameURL(this ILearningMaterialLinkService lm)
        {
            return lm.LinkName.Replace(" ", "-");
        }
    }

    public static class CategoryExtensions
    {
        public static string GetNameUrl(this ICategoryLink category)
        {
            return category.Name.Replace(" ", "-");
        }
    }

    public static class CourseExtensions
    {
        public static string GetNameUrl(this ICourseLink category)
        {
            return category.Name.Replace(" ", "-");
        }
    }

    public static class LessonExtensions
    {
        public static string GetNameUrl(this ILessonLink lesson)
        {
            return lesson.Name.Replace(" ", "-");
        }
    }
}
