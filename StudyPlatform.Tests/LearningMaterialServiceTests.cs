using Microsoft.EntityFrameworkCore;
using Moq;
using StudyPlatform.Data;
using StudyPlatform.Data.Models;
using StudyPlatform.Services.Category;
using StudyPlatform.Services.Course;
using StudyPlatform.Services.LearningMaterial;
using StudyPlatform.Services.Lesson;
using StudyPlatform.Services.TeacherLesson;
using static StudyPlatform.Common.GeneralConstants;

namespace StudyPlatform.Tests
{
    [TestFixture]
    public class LearningMaterialServiceTests
    {
        private StudyPlatformDbContext dbContext;
        private Mock<LearningMaterialService> _learningMaterialServiceMock;

        private int defaultCourseId = 1;
        private string defaultCourseName = "In Memory Course 1";
        private int defaultLessonId = 1;
        private string defaultLessonName = "In Memory Lesson 1";
        private int defaultLearningMaterialId = 1;
        private string defaultLearningMaterialName = "In Memory Learning Material 1";
        private string defaultLearningMaterialFileName = "In Memory File Name 1.pdf";
        private Guid defaultGuid;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Arrange
            defaultGuid = Guid.NewGuid();

            ICollection<Course> courseData = new List<Course>()
            {
                new Course{Id = 1,
                        Name = defaultCourseName,
                        CategoryId = 1},
            };

            ICollection<Lesson> lessonData = new List<Lesson>()
            {
                new Lesson{Id = defaultLessonId,
                        Name = defaultLessonName,
                        CourseId = defaultCourseId},
            };

            ICollection<LearningMaterial> lmData = new List<LearningMaterial>()
            {
                new LearningMaterial{Id = defaultLearningMaterialId,
                                    LearningMaterialName = defaultLearningMaterialName,
                                    FileName = defaultLearningMaterialFileName,
                                    LessonId = defaultLessonId}
            };

            var dbOptions = new DbContextOptionsBuilder<StudyPlatformDbContext>()
                .UseInMemoryDatabase(databaseName: "LearningMaterialServiceTests_InMemory")
                .Options;
            this.dbContext = new StudyPlatformDbContext(dbOptions);
            this.dbContext.Courses.AddRange(courseData);
            this.dbContext.Lessons.AddRange(lessonData);
            this.dbContext.LearningMaterials.AddRange(lmData);
            this.dbContext.SaveChanges();
        }

        [SetUp]
        public void Setup()
        {
            this._learningMaterialServiceMock = new Mock<LearningMaterialService>(this.dbContext);
        }

        [Test]
        public async Task LearningMaterialService_AnyByIdAsync_ShouldReturnTrueIfIdIsValid()
        {
            bool result = await this._learningMaterialServiceMock.Object.AnyByIdAsync(defaultLearningMaterialId);
            Assert.True(result);
        }

        [Test]
        public async Task LearningMaterialService_AnyByIdAsync_ShouldReturnFalseIfIdIsValid()
        {
            bool result = await this._learningMaterialServiceMock.Object.AnyByIdAsync(-1);
            Assert.False(result);
        }
        [Test]
        public async Task LearningMaterialService_AnyByNameAsync_ShouldReturnTrueIfIdIsValid()
        {
            bool result = await this._learningMaterialServiceMock.Object.AnyByNameAsync(defaultLearningMaterialName);
            Assert.True(result);
        }

        [Test]
        public async Task LearningMaterialService_AnyByNameAsync_ShouldReturnFalseIfIdIsValid()
        {
            bool result = await this._learningMaterialServiceMock.Object.AnyByNameAsync(string.Empty);
            Assert.False(result);
        }

        [Test]
        public async Task LearningMaterialService_GetAllLmModelsByLessonAsync_ShouldReturnValidCollectionIfIdIsValid()
        {
            var result = await this._learningMaterialServiceMock.Object.GetAllLmModelsByLessonAsync(defaultLessonId);

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            Assert.True(result.First().Id == defaultLearningMaterialId);
        }
        [Test]
        public void LearningMaterialService_GetAllLmModelsByLesson_ShouldReturnValidCollectionIfIdIsValid()
        {
            var result = this._learningMaterialServiceMock.Object.GetAllLmModelsByLesson(defaultLessonId);

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            Assert.True(result.First().Id == defaultLearningMaterialId);
            Assert.True(result.First().FileName == defaultLearningMaterialFileName);
            Assert.False(result.First().LinkName.Contains(".pdf"));
        }
        [Test]
        public async Task LearningMaterialService_GetIdByNameAsync_ShouldReturnIdIfNameIsValid()
        {
            int lessonIdToCompare = 1;
            int result = await this._learningMaterialServiceMock.Object.GetIdByNameAsync(defaultLearningMaterialName);

            Assert.AreEqual(result, lessonIdToCompare);
        }
        [Test]
        public async Task LearningMaterialService_GetViewModelAsync_ShouldReturnViewModelIfIdIsValid()
        {
            var result = await this._learningMaterialServiceMock.Object.GetViewModelAsync(defaultLearningMaterialId);

            Assert.NotNull(result);
            Assert.True(result.Id == defaultLearningMaterialId);
            Assert.True(result.FileName == defaultLearningMaterialFileName);
            Assert.False(result.LinkName.Contains(".pdf"));
            Assert.True(result.FullPath == (LmFilePath + result.FileName));
            Assert.True(result.Title == defaultLearningMaterialName);
            Assert.True(result.LessonLink.LessonId == defaultLessonId);
            Assert.True(result.LessonLink.LessonName == defaultLessonName);
            Assert.False(result.LessonLink.LessonNameLink.Contains(" "));
        }
    }
}
