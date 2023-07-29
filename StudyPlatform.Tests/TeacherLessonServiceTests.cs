using Microsoft.EntityFrameworkCore;
using Moq;
using StudyPlatform.Data.Models;
using StudyPlatform.Data;
using StudyPlatform.Services.Category;
using StudyPlatform.Services.Course;
using StudyPlatform.Services.LearningMaterial;
using StudyPlatform.Services.Lesson;
using StudyPlatform.Services.TeacherLesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Tests
{
    [TestFixture]
    public class TeacherLessonServiceTests
    {
        private StudyPlatformDbContext dbContext;
        private Mock<TeacherLessonService> _teacherLessonServiceMock;
        private ApplicationUser _fakeUser;

        private int defaultCourseId = 1;
        private int defaultLessonId = 1;
        private string defaultLessonName = "In Memory Lesson 1";
        private int defaultLearningMaterialId = 1;
        private string defaultLearningMaterialName = "In Memory Learning Material 1";
        private string defaultLearningMaterialFileName = "In Memory File Name 1";
        private Guid defaultGuid;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            //Arrange
            defaultGuid = Guid.NewGuid();

            this._fakeUser = new ApplicationUser();
            this._fakeUser.UserName = "Fake Username";
            this._fakeUser.FirstName = "Fake First Name";
            this._fakeUser.MiddleName = "Fake Middle Name";
            this._fakeUser.LastName = "Fake Last Name";
            this._fakeUser.Age = "50";
            this._fakeUser.Email = "fakeuser@test.com";

            ICollection<Lesson> lessonData = new List<Lesson>()
            {
                new Lesson{Id = defaultLessonId,
                        Name = defaultLessonName,
                        CourseId = defaultCourseId}
            };

            ICollection<LearningMaterial> lmData = new List<LearningMaterial>()
            {
                new LearningMaterial{Id = defaultLearningMaterialId,
                                    LearningMaterialName = defaultLearningMaterialName,
                                    FileName = defaultLearningMaterialFileName,
                                    LessonId = defaultLessonId}
            };

            ICollection<Teacher> teacherData = new List<Teacher>()
            {
                new Teacher{ Id = defaultGuid }
            };


            var dbOptions = new DbContextOptionsBuilder<StudyPlatformDbContext>()
                .UseInMemoryDatabase(databaseName: "TeacherLessonServiceTests_InMemory")
                .Options;
            this.dbContext = new StudyPlatformDbContext(dbOptions);
            this.dbContext.Users.AddRange(_fakeUser);
            this.dbContext.Lessons.AddRange(lessonData);
            this.dbContext.LearningMaterials.AddRange(lmData);
            this.dbContext.Teachers.AddRange(teacherData);
            this.dbContext.SaveChanges();

            Guid teacherId = this.dbContext.Users.First().Id;
            this.dbContext.TeacherLessons.Add(new TeacherLesson { TeacherId = teacherId, LessonId = defaultLessonId });
            this.dbContext.SaveChanges();

            defaultGuid = teacherId;
        }

        [SetUp]
        public void Setup()
        {
            this._teacherLessonServiceMock = new Mock<TeacherLessonService>(this.dbContext);
        }

        [Test]
        public async Task TeacherLessonServicec_TeacherLessonAlreadyExists_ShouldReturnTrueIfIdsAreValid()
        {
            bool result = await this._teacherLessonServiceMock.Object.TeacherLessonAlreadyExists(defaultLessonId, defaultGuid);

            Assert.True(result);
        }
        [Test]
        public async Task TeacherLessonServicec_GetLessonIdByTeacherGuidAsync_ShouldReturnLessonIdIfTeacherIdIsValid()
        {
            int result = await this._teacherLessonServiceMock.Object.GetLessonIdByTeacherGuidAsync(defaultGuid);

            Assert.AreEqual(result, defaultLessonId);
        }
        [Test]
        public async Task TeacherLessonServicec_GetTeacherIdByLessonIdAsync_ShouldReturnTeacherIdIfLessonIdIsValid()
        {
            Guid result = await this._teacherLessonServiceMock.Object.GetTeacherIdByLessonIdAsync(defaultLessonId);

            Assert.AreEqual(result, defaultGuid);
        }
        [Test]
        public async Task TeacherLessonServicec_AddAsync_ShouldAddNewTeacherLessonsEntityInDb()
        {
            int tlCount = this.dbContext.TeacherLessons.Count();
            Guid newTeacherId = Guid.NewGuid();
            int newLessonId = 2;

            await this._teacherLessonServiceMock.Object.AddAsync(newTeacherId, newLessonId);

            bool teacherExists = await this.dbContext.TeacherLessons.AnyAsync(t => t.TeacherId == newTeacherId);
            bool lessonExists = await this.dbContext.TeacherLessons.AnyAsync(t => t.LessonId == newLessonId);

            Assert.That(this.dbContext.TeacherLessons.Count(), Is.EqualTo(tlCount + 1));
            Assert.True(teacherExists);
            Assert.True(teacherExists);
        }
        [Test]
        public async Task TeacherLessonServicec_GetTeachersForLessonAsync_ShouldReturnListIfIdIsValid()
        {
            var result = await this._teacherLessonServiceMock.Object.GetTeachersForLessonAsync(defaultLessonId);

            Assert.NotNull(result);
            Assert.That(result.Count > 0);
            Assert.AreEqual(result.First().Id, this.dbContext.Users.First().Id);
        }

    }
}
