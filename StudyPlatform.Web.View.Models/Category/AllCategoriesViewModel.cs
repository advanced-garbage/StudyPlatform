using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Web.View.Models.Category
{
    public class AllCategoriesViewModel
    {
        public AllCategoriesViewModel() { 
            this.Categories = new List<CategoryViewModel>();
        }

        public bool? IsViewedByTeacher { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}
