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
    public class LessonFormServiceTests
    {
        private StudyPlatformDbContext dbContext;
        private Mock<CourseViewService> _courseViewServiceMock;
        private Mock<LearningMaterialService> _learningMaterialServiceMock;
        private Mock<TeacherLessonService> _teacherLessonServiceMock;
        private Mock<CategoryViewService> _categoryViewServiceMock;
        private Mock<LessonViewService> _lessonViewServiceMock;
        private Mock<LessonFormService> _lessonFormServiceMock;

        private int defaultCourseId = 1;
        private string defaultCourseName = "In Memory Course 1";
        private string defaultCourseDescription = "In Memory Course 1 Description";
        private int defaultLessonId = 1;
        private string defaultLessonName = "In Memory Lesson 1";
        private string defaultLessonDescription = "In Memory Lesson 1 Description";
        private int defaultLearningMaterialId = 1;
        private string defaultLearningMaterialName = "In Memory Learning Material 1";
        private string defaultLearningMaterialFileName = "In Memory File Name 1";
        private Guid defaultGuid;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            defaultGuid = Guid.NewGuid();
            //Arrange
            ICollection<Category> categoryData = new List<Category>()
            {
                new Category{Id = 1, Name = "In Memory Category 1"},
            };

            ICollection<Course> courseData = new List<Course>()
            {
                new Course{Id = 1,
                        Name = defaultCourseName,
                        Description = defaultCourseDescription,
                        CategoryId = 1},
                new Course{Id = 2,
                        Name = defaultCourseName.Replace('1', '2'),
                        Description = defaultCourseDescription.Replace('1', '2'),
                        CategoryId = 1}
            };

            ICollection<Lesson> lessonData = new List<Lesson>()
            {
                new Lesson{Id = defaultLessonId,
                        Name = defaultLessonName,
                        Description = defaultLessonDescription,
                        CourseId = defaultCourseId},
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
                .UseInMemoryDatabase(databaseName: "LessonFormServiceTestInMemory")
                .Options;
            this.dbContext = new StudyPlatformDbContext(dbOptions);
            this.dbContext.Categories.AddRange(categoryData);
            this.dbContext.Courses.AddRange(courseData);
            this.dbContext.Lessons.AddRange(lessonData);
            this.dbContext.LearningMaterials.AddRange(lmData);
            this.dbContext.Teachers.AddRange(teacherData);
            this.dbContext.TeacherLessons.Add(new TeacherLesson { TeacherId = teacherData.First().Id, LessonId = defaultLessonId });
            this.dbContext.SaveChanges();
        }

        [SetUp]
        public void Setup()
        {
            this._categoryViewServiceMock = new Mock<CategoryViewService>(this.dbContext);
            this._courseViewServiceMock = new Mock<CourseViewService>(this.dbContext,
                                                                      this._categoryViewServiceMock.Object);
            this._learningMaterialServiceMock = new Mock<LearningMaterialService>(this.dbContext);
            this._teacherLessonServiceMock = new Mock<TeacherLessonService>(this.dbContext);
            this._lessonViewServiceMock = new Mock<LessonViewService>(this.dbContext,
                                                                      _learningMaterialServiceMock.Object,
                                                                      _courseViewServiceMock.Object,
                                                                      _categoryViewServiceMock.Object,
                                                                      _teacherLessonServiceMock.Object);
            this._lessonFormServiceMock = new Mock<LessonFormService>(this.dbContext,
                                                                      this._courseViewServiceMock.Object);
        }

        [Test]
        public async Task LessonForm_GetFormByIdAsync_ShouldReturnFormModelIfIdIsValid()
        {
            var resultModel = await this._lessonFormServiceMock.Object.GetFormByIdAsync(defaultLessonId);
            
            Assert.IsNotNull(resultModel);
            Assert.That(resultModel.Id, Is.EqualTo(defaultLessonId));
            Assert.That(resultModel.Name, Is.EqualTo(defaultLessonName));
        }

        [Test]
        public async Task LessonForm_AddAsync_ShouldAddLessonEntityToDb()
        {
            int lessonsCount = this.dbContext.Lessons.Count();
            var formModel = await this._lessonFormServiceMock.Object.GetFormByIdAsync(defaultLessonId);
            formModel.Id = 2;
            formModel.Name = "In Memory Lesson 2";
            formModel.Description = "In Memory Lesson 2 Description";

            await this._lessonFormServiceMock.Object.AddAsync(formModel);
            bool resultExists = await this._lessonViewServiceMock.Object.AnyByIdAsync(formModel.Id);   

            Assert.That(this.dbContext.Lessons.Count, Is.EqualTo(lessonsCount + 1));
            Assert.True(resultExists);
        }

        [Test]
        public async Task LessonForm_EditAsync_ShouldEditDataFromExistingLesson()
        {
            var oldModel = await this._lessonViewServiceMock.Object.GetLessonByIdAsync(defaultLessonId);

            var formModel = await this._lessonFormServiceMock.Object.GetFormByIdAsync(defaultLessonId);
            formModel.Name = "In Memory Lesson 1 EDITED";
            formModel.Description = "In Memory Lesson 1 Description EDITED";
            formModel.CourseId = 2;

            await this._lessonFormServiceMock.Object.EditAsync(formModel);

            var newModel = await this._lessonViewServiceMock.Object.GetLessonByIdAsync(defaultLessonId);

            Assert.AreEqual(oldModel.Id, newModel.Id);
            Assert.True(oldModel.Name != newModel.Name);
            Assert.True(oldModel.Description != newModel.Description);
            Assert.True(oldModel.CourseId != newModel.CourseId);
        }
        [Test]
        public async Task LessonForm_RemoveAsync_ShouldRemoveLessonEntityIfIdIsValid()
        {
            var stillExists = await this._lessonViewServiceMock.Object.AnyByIdAsync(defaultLessonId);
            Assert.True(stillExists);

            await this._lessonFormServiceMock.Object.RemoveAsync(defaultLessonId);

            var isNowDeleted = !await this._lessonViewServiceMock.Object.AnyByIdAsync(defaultLessonId);
            Assert.True(isNowDeleted);
        }

    }
}
