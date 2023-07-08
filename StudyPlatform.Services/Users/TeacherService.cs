using StudyPlatform.Data;
using StudyPlatform.Services.Users.Interfaces;
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
    }
}
