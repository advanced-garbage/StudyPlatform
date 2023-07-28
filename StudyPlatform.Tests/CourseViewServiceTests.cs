using Microsoft.EntityFrameworkCore;
using Moq;
using StudyPlatform.Data;
using StudyPlatform.Services.Category;
using StudyPlatform.Services.Course;
using StudyPlatform.Web.View.Models.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Tests
{
    [TestFixture]
    public class CourseViewServiceTests
    {
        private StudyPlatformDbContext dbContext;
        private Mock<CourseViewService> _courseViewServiceMock;
        private Mock<CategoryViewService> _categoryViewServiceMock;

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
        public async Task CourseView_GetCategoryIdByCourseIdAsyncShouldReturnCategoryIdIfCourseIdExists()
        {
            int categoryIdToCompare = 1;
            int courseId = 2;

            int resultCategoryId = await this._courseViewServiceMock.Object.GetCategoryIdByCourseIdAsync(courseId);

            Assert.That(resultCategoryId, Is.EqualTo(categoryIdToCompare));
        }

        [Test]
        public async Task CourseView_GetCategoryIdByCourseIdAsyncShouldReturnDefaultValueIfCourseIdIsInvalid()
        {
            int courseId = -1;

            int resultCategoryId = await this._courseViewServiceMock.Object.GetCategoryIdByCourseIdAsync(courseId);

            Assert.That(resultCategoryId, Is.EqualTo(0));
        }

        [Test]
        public async Task CourseView_GetByIdReturnsCourseViewModelWhenIdIsValid()
        {
            int courseId = 1;

            CourseViewModel resultModel = await this._courseViewServiceMock.Object.GetById(courseId);

            Assert.NotNull(resultModel);
            Assert.That(resultModel.Id, Is.EqualTo(courseId));
        }

        [Test]
        public async Task CourseView_GetByIdReturnsNullWhenIdIsInvalid()
        {
            int courseId = -1;

            CourseViewModel? resultModel = await this._courseViewServiceMock.Object.GetById(courseId);

            Assert.Null(resultModel);
        }

        [Test]
        public async Task CourseView_GetFormCourseAsyncShouldReturnFormModelWhenIdIsValid()
        {
            int courseId = 1;

            CourseViewFormModel? resultModel = await this._courseViewServiceMock.Object.GetFormCourseAsync(courseId);

            Assert.NotNull(resultModel);
            Assert.AreEqual(courseId, resultModel.Id);
            Assert.That(resultModel.Categories.Count > 0);
        }

        [Test]
        public async Task CourseView_GetFormCourseAsyncThrowsErrorWhenIdIsInvalid()
        {
            int courseId = -1;

            Assert.ThrowsAsync<InvalidOperationException>(async () => await this._courseViewServiceMock.Object.GetFormCourseAsync(courseId));
        }

        [Test]
        public async Task CourseView_GetNameByIdAsyncReturnsCourseNameWhenIdIsInvalid()
        {
            int courseId = 1;
            string courseNameToTest = "In Memory Course 1";

            string result = await this._courseViewServiceMock.Object.GetNameByIdAsync(courseId);

            Assert.NotNull(result);
            Assert.AreEqual(result, courseNameToTest);
        }

        [Test]
        public async Task CourseView_GetNameByIdAsyncThrowsErrorWhenIdIsInvalid()
        {
            int courseId = -1;

            Assert.ThrowsAsync<InvalidOperationException>(async () => await this._courseViewServiceMock.Object.GetNameByIdAsync(courseId));
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
            int courseId = -1;

            bool result = await this._courseViewServiceMock.Object.AnyByIdAsync(courseId);

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
            int idToTest = 1;
            int courseIdResult = await this._courseViewServiceMock.Object.GetIdAsync(idToTest);

            Assert.AreEqual(idToTest, courseIdResult);
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
            int courseId = 1;
            string courseNameToTest = "In Memory Course 1";

            string result = await this._courseViewServiceMock.Object.GetNameByIdAsync(courseId);

            Assert.AreEqual(result, courseNameToTest);
        }

        [Test]
        public async Task CourseView_GetNameUrlByIdAsyncShouldReturnNameUrlWithNoSpacesWhenIdIsValid()
        {
            int courseId = 1;
            string courseNameToTest = "In Memory Course 1".Replace(" ", "-");

            string result = await this._courseViewServiceMock.Object.GetNameUrlByIdAsync(courseId);

            Assert.AreEqual(result, courseNameToTest);
        }

        [Test]
        public async Task CourseView_GetIdAsyncShouldReturnValidIdWhenNameIsValid()
        {
            int courseIdToCompare = 1;
            string courseName = "In Memory Course 1";

            int result = await this._courseViewServiceMock.Object.GetIdByNameAsync(courseName);

            Assert.AreEqual(courseIdToCompare, result);
        }
    }
}
