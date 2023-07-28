using Microsoft.EntityFrameworkCore;
using Moq;
using StudyPlatform.Data;
using StudyPlatform.Services.Category;
using StudyPlatform.Services.Course;
using StudyPlatform.Web.View.Models.Course;

namespace StudyPlatform.Tests
{
    [TestFixture]
    public class CourseViewServiceTests
    {
        private StudyPlatformDbContext dbContext;
        private Mock<CourseViewService> _courseViewServiceMock;
        private Mock<CategoryViewService> _categoryViewServiceMock;
        private int defaultCourseId = 1;
        private string defaultCourseName = "In Memory Course 1";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ICollection<Data.Models.Category> categoryData = new List<Data.Models.Category>()
            {
                new Data.Models.Category{Id = 1, Name = "In Memory Category 1"},
            };

            //Arrange
            ICollection<Data.Models.Course> courseData = new List<Data.Models.Course>()
            {
                new Data.Models.Course{Id = 1, Name = "In Memory Course 1", Description = "In Memory Course 1 Description", CategoryId = 1},
                new Data.Models.Course{Id = 2, Name = "In Memory Course 2", Description = "In Memory Course 2 Description", CategoryId = 1}
            };

            var dbOptions = new DbContextOptionsBuilder<StudyPlatformDbContext>()
                .UseInMemoryDatabase(databaseName: "CourseServiceTestInMemory")
                .Options;

            this.dbContext = new StudyPlatformDbContext(dbOptions);
            this.dbContext.Categories.AddRange(categoryData);
            this.dbContext.Courses.AddRange(courseData);
            this.dbContext.SaveChanges();
        }

        [SetUp]
        public void Setup()
        {
            _categoryViewServiceMock = new Mock<CategoryViewService>(this.dbContext);
            _courseViewServiceMock = new Mock<CourseViewService>(this.dbContext, _categoryViewServiceMock.Object);
        }

        [Test]
        public async Task CourseView_GetCategoryIdByCourseIdAsync_ShouldReturnCategoryIdIfCourseIdExists()
        {
            int categoryIdToCompare = 1;

            int resultCategoryId = await this._courseViewServiceMock.Object.GetCategoryIdByCourseIdAsync(defaultCourseId);

            Assert.That(resultCategoryId, Is.EqualTo(categoryIdToCompare));
        }

        [Test]
        public async Task CourseView_GetCategoryIdByCourseIdAsync_ShouldReturnDefaultValueIfCourseIdIsInvalid()
        {
            int resultCategoryId = await this._courseViewServiceMock.Object.GetCategoryIdByCourseIdAsync(-1);

            Assert.That(resultCategoryId, Is.EqualTo(0));
        }

        [Test]
        public async Task CourseView_GetById_ReturnsCourseViewModelWhenIdIsValid()
        {
            CourseViewModel resultModel = await this._courseViewServiceMock.Object.GetById(defaultCourseId);

            Assert.NotNull(resultModel);
            Assert.That(resultModel.Id, Is.EqualTo(defaultCourseId));
        }

        [Test]
        public async Task CourseView_GetById_ReturnsNullWhenIdIsInvalid()
        {
            CourseViewModel? resultModel = await this._courseViewServiceMock.Object.GetById(-1);

            Assert.Null(resultModel);
        }

        [Test]
        public async Task CourseView_GetFormCourseAsync_ShouldReturnFormModelWhenIdIsValid()
        {
            CourseViewFormModel? resultModel = await this._courseViewServiceMock.Object.GetFormCourseAsync(defaultCourseId);

            Assert.NotNull(resultModel);
            Assert.AreEqual(defaultCourseId, resultModel.Id);
            Assert.That(resultModel.Categories.Count > 0);
        }

        [Test]
        public async Task CourseView_GetFormCourseAsyncThrowsErrorWhenIdIsInvalid()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () => await this._courseViewServiceMock.Object.GetFormCourseAsync(-1));
        }

        [Test]
        public async Task CourseView_GetNameByIdAsyncReturnsCourseNameWhenIdIsInvalid()
        {
            string result = await this._courseViewServiceMock.Object.GetNameByIdAsync(defaultCourseId);

            Assert.NotNull(result);
            Assert.AreEqual(result, defaultCourseName);
        }

        [Test]
        public async Task CourseView_GetNameByIdAsyncThrowsErrorWhenIdIsInvalid()
        {Assert.ThrowsAsync<InvalidOperationException>(async () => await this._courseViewServiceMock.Object.GetNameByIdAsync(-1));
        }

        [Test]
        public async Task CourseView_AnyByIdAsyncReturnsTrueIfIdIsValid()
        {
            int courseId = 1;

            bool result = await this._courseViewServiceMock.Object.AnyByIdAsync(courseId);

            Assert.True(result);
        }

        [Test]
        public async Task CourseView_AnyByIdAsyncReturnsFalseIfIdIsValid()
        {
            bool result = await this._courseViewServiceMock.Object.AnyByIdAsync(-1);

            Assert.False(result);
        }

        [Test]
        public async Task CourseView_GetAllAsyncShouldReturnEveryExistingCourse()
        {
            var result = await this._courseViewServiceMock.Object.GetAllAsync();

            Assert.That(result.Count >= 2);
        }

        [Test]
        public async Task CourseView_GetIdAsyncShouldReturnCourseIdWhenIdIsValid()
        {
            int courseIdResult = await this._courseViewServiceMock.Object.GetIdAsync(defaultCourseId);

            Assert.AreEqual(defaultCourseId, courseIdResult);
        }

        [Test]
        public async Task CourseView_GetIdAsyncShouldReturnDefaultWhenIdIsInvalid()
        {
            int courseIdResult = await this._courseViewServiceMock.Object.GetIdAsync(-1);

            Assert.AreEqual(0, courseIdResult);
        }
        [Test]
        public async Task CourseView_GetIdByNameAsyncShouldReturnCourseNameWhenIdIsValid()
        {
            string result = await this._courseViewServiceMock.Object.GetNameByIdAsync(defaultCourseId);

            Assert.AreEqual(result, defaultCourseName);
        }

        [Test]
        public async Task CourseView_GetNameUrlByIdAsyncShouldReturnNameUrlWithNoSpacesWhenIdIsValid()
        {
            string courseNameToTest = defaultCourseName.Replace(" ", "-");

            string result = await this._courseViewServiceMock.Object.GetNameUrlByIdAsync(defaultCourseId);

            Assert.AreEqual(result, courseNameToTest);
        }

        [Test]
        public async Task CourseView_GetIdAsyncShouldReturnValidIdWhenNameIsValid()
        {
            int result = await this._courseViewServiceMock.Object.GetIdByNameAsync(defaultCourseName);

            Assert.AreEqual(defaultCourseId, result);
        }
    }
}
