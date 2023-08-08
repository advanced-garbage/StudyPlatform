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
        /// <summary>
        /// Returns whether a teacher by this Guid exists in the Database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> AnyById(Guid id);

    }
}
