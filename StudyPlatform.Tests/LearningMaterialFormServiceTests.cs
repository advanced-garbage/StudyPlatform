using Microsoft.EntityFrameworkCore;
using Moq;
using StudyPlatform.Data.Models;
using StudyPlatform.Data;
using StudyPlatform.Services.Category;
using StudyPlatform.Services.Course;
using StudyPlatform.Services.LearningMaterial;
using StudyPlatform.Services.Lesson;
using StudyPlatform.Services.TeacherLesson;
using static StudyPlatform.Common.GeneralConstants;
using StudyPlatform.Web.View.Models.LearningMaterial;
using Microsoft.AspNetCore.Http;

namespace StudyPlatform.Tests
{
    [TestFixture]
    public class LearningMaterialFormServiceTests
    {
        private StudyPlatformDbContext dbContext;
        private Mock<LearningMaterialService> _learningMaterialServiceMock;
        private Mock<LearningMaterialFormService> _learningMaterialFormServiceMock;

        private int defaultCourseId = 1;
        private string defaultCourseName = "In Memory Course 1";
        private int defaultLessonId = 1;
        private string defaultLessonName = "In Memory Lesson 1";
        private int defaultLearningMaterialId = 1;
        private string defaultLearningMaterialName = "In Memory Learning Material 1";
        private string defaultLearningMaterialFileName = "In Memory File Name 1.pdf";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {

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
                .UseInMemoryDatabase(databaseName: "LearningMaterialFormServiceTests_InMemory")
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
            this._learningMaterialFormServiceMock = new Mock<LearningMaterialFormService>(this.dbContext);
        }

        [Test]
        public async Task LessonForm_AddLearningMaterial_ShouldAddNewLmEntityInDb()
        {
            int newLearningMaterialId = 2;
            string formModelLmName = "Test Learning Material";
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("Mock File 1.pdf");
            mockFile.Setup(f => f.ContentType).Returns("application/pdf");

            UploadLearningMaterialFormModel formModel
                = new UploadLearningMaterialFormModel()
                {
                    LearningMaterialName = formModelLmName,
                    File = mockFile.Object,
                    LessonId = defaultLessonId,
                    LessonName = defaultLessonName,
                    CourseName = defaultCourseName
                };

            await this._learningMaterialFormServiceMock.Object.AddLearningMaterial(formModel);
            bool result = await this._learningMaterialServiceMock.Object.AnyByIdAsync(newLearningMaterialId);
            var resultModel = await this._learningMaterialServiceMock.Object.GetViewModelAsync(newLearningMaterialId);

            Assert.True(result);
            Assert.AreEqual(resultModel.Id, newLearningMaterialId);
            Assert.AreEqual(resultModel.FileName, mockFile.Object.FileName);
            Assert.AreEqual(resultModel.FullPath, LmFilePath + resultModel.FileName);
        }
    }
}
