using Castle.Core.Configuration;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Moq;
using StudyPlatform.Data;
using StudyPlatform.Data.Models;
using StudyPlatform.Services.Category;
using StudyPlatform.Services.Course;
using StudyPlatform.Services.LearningMaterial;
using StudyPlatform.Services.Lesson;
using StudyPlatform.Services.TeacherLesson;

namespace StudyPlatform.Tests
{
    [TestFixture]
    public class LessonViewServiceTests
    {
        private StudyPlatformDbContext dbContext;
        private Mock<CourseViewService> _courseViewServiceMock;
        private Mock<LearningMaterialService> _learningMaterialServiceMock;
        private Mock<TeacherLessonService> _teacherLessonServiceMock;
        private Mock<CategoryViewService> _categoryViewServiceMock;
        private Mock<LessonViewService> _lessonViewServiceMock;

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
                .UseInMemoryDatabase(databaseName: "LessonViewServiceTests_InMemory")
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
        }

        [Test]
        public async Task LessonView_AnyByIdAsync_ShouldReturnTrueIfIdIsValid()
        {
            bool result = await this._lessonViewServiceMock.Object.AnyByIdAsync(defaultLessonId);

            Assert.True(result);
        }

        [Test]
        public async Task LessonView_AnyByIdAsync_ShouldReturnFalseIfIdIsInvalid()
        {
            bool result = await this._lessonViewServiceMock.Object.AnyByIdAsync(-1);

            Assert.False(result);
        }

        [Test]
        public async Task LessonView_AnyByNameAsync_ShouldReturnTrueIfIdIsValid()
        {
            bool result = await this._lessonViewServiceMock.Object.AnyByNameAsync(defaultLessonName);

            Assert.True(result);
        }

        [Test]
        public async Task LessonView_AnyByNameAsync_ShouldReturnFalseIfIdIsInvalid()
        {
            bool result = await this._lessonViewServiceMock.Object.AnyByNameAsync(string.Empty);

            Assert.False(result);
        }

        [Test]
        public async Task LessonView_GetAccountCreditsAsync_ShouldReturnAListWithALessonElementIfIdIsValid()
        {
            var testModel = await this._lessonViewServiceMock.Object.GetAccountCreditsAsync(defaultGuid);

            Assert.NotNull(testModel);
            Assert.True(testModel.AccountCategories.Count() > 0);
            Assert.True(testModel.AccountCourses.Count() > 0);
            Assert.True(testModel.AccountLessons.Count() > 0);
            Assert.That(testModel.AccountLessons.First().Id, Is.EqualTo(defaultLessonId));
        }

        [Test]
        public async Task LessonView_GetAllLessonsByCourseIdAsync_ShouldReturnAListWithElementsIfIdIsValid()
        {
            var resultModel = await this._lessonViewServiceMock.Object.GetAllLessonsByCourseIdAsync(defaultCourseId);

            Assert.NotNull(resultModel);
            Assert.That(resultModel.Count() > 0);
            Assert.That(resultModel.First().Id, Is.EqualTo(defaultLessonId));
            Assert.That(resultModel.First().CourseId ,Is.EqualTo(defaultCourseId));
        }
        
        [Test]
        public async Task LessonView_GetCourseIdByLessonId_ShouldReturnCourseIdIfLessonIdIsValid()
        {
            int resultCourseId = await this._lessonViewServiceMock.Object.GetCourseIdByLessonId(defaultLessonId);

            Assert.That(resultCourseId, Is.EqualTo(defaultCourseId));
        }

        [Test]
        public async Task LessonView_GetLessonByIdAsync_ShouldReturnLessonViewModelIfIdIsValid()
        {
            var resultModel = await this._lessonViewServiceMock.Object.GetLessonByIdAsync(defaultLessonId);

            Assert.NotNull(resultModel);
            Assert.That(resultModel.Id, Is.EqualTo(defaultLessonId));
        }
        
        [Test]
        public async Task LessonView_GetLessonIdByLearningMaterialId_ShouldReturnLessonIdIfLmIdIsValid()
        {
            int resultLessonId = await this._lessonViewServiceMock.Object.GetLessonIdByLearningMaterialId(defaultLearningMaterialId);

            Assert.AreEqual(resultLessonId, defaultLessonId);
        }
        [Test]
        public async Task LessonView_GetNameByIdAsync_ShouldReturnLessonNameIfIdIsValid()
        {
            string resultLessonName = await this._lessonViewServiceMock.Object.GetNameByIdAsync(defaultLessonId);

            Assert.AreEqual(resultLessonName, defaultLessonName);
        }
    }
}
