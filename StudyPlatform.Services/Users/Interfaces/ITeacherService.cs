using StudyPlatform.Web.View.Models.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Services.Users.Interfaces
{
    public interface ITeacherService
    {
        Task<bool> AnyById(Guid id);

        Task<TeacherViewModel> GetTeacher(Guid id);
    }
}
