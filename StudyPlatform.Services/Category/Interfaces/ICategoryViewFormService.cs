using StudyPlatform.Web.View.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Services.Category.Interfaces
{
    public interface ICategoryViewFormService
    {
        public Task AddAsync(CategoryViewFormModel model);

        public Task RemoveAsync(int id);

        public Task EditAsync(CategoryViewFormModel model);
    }
}
