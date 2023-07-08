﻿using Microsoft.EntityFrameworkCore;
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
    public class TeacherService : ITeacherService
    {
        private readonly StudyPlatformDbContext _db;

        public TeacherService(StudyPlatformDbContext db)
        {
            this._db = db;
        }

        public async Task<bool> AnyById(Guid id)
        {
            bool teacherExists
                = await this._db
                .Teachers
                .AnyAsync(u => u.Id.Equals(id));

            return teacherExists;
        }

        public async Task<TeacherViewModel> GetTeacher(Guid id)
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
