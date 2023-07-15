using StudyPlatform.Web.View.Models.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Services.Course.Interfaces
{
    public interface ICourseViewFormService
    {
        public Task EditAsync(CourseViewFormModel model);

        public Task RemoveAsync(int id);

        public Task AddAsync(CourseViewFormModel model);

        public Task AssignToNewCategoryAsync(int courseId, int categoryId);
    }
}
