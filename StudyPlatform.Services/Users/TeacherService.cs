using Microsoft.EntityFrameworkCore;
using StudyPlatform.Data;
using StudyPlatform.Services.Users.Interfaces;
using StudyPlatform.Web.View.Models.Teacher;
using StudyPlatform.Web.View.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Services.Users
{
    /// <summary>
    /// Teacher Dependency mainly used for reading operations.
    /// </summary>
    public class TeacherService : ITeacherService
    {
        /// <summary>
        /// DbContext dependency.
        /// </summary>
        private readonly StudyPlatformDbContext _db;

        public TeacherService(StudyPlatformDbContext db)
        {
            this._db = db;
        }

        /// <summary>
        /// Searches for a Teacher Entity with the given Guid. Returns a bool.
        /// </summary>
        public async Task<bool> AnyById(Guid id)
        {
            bool teacherExists
                = await this._db
                .Teachers
                .AnyAsync(u => u.Id.Equals(id));

            return teacherExists;
        }
        /// <summary>
        /// Returns a collection of every teacher model
        /// </summary>
        public async Task<ICollection<TeacherViewModel>> GetAllAsync()
        {
            ICollection<TeacherViewModel> teachers
                = await this._db
                .Users
                .Select(u => new TeacherViewModel()
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    MiddleName = u.MiddleName,
                    LastName = u.LastName,
                    Age = u.Age,
                    Role = "Teacher"
                })
                .ToListAsync();

            return teachers;
        }

        /// <summary>
        /// Returns a Teacher View Model with the given Guid.
        /// </summary>
        public async Task<TeacherViewModel> GetAsync(Guid id)
        {
            TeacherViewModel userModel
                = await this._db
                .Users
                .Where(u => u.Id.Equals(id))
                .Select(u => new TeacherViewModel()
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    MiddleName = u.MiddleName,
                    LastName = u.LastName,
                    Age = u.Age,
                    Role = "Teacher"
                })
                .FirstOrDefaultAsync();

            return userModel;
        }
    }
}
