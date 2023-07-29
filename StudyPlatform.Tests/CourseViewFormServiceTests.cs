using Microsoft.EntityFrameworkCore;
using Moq;
using StudyPlatform.Data;
using StudyPlatform.Services.Category;
using StudyPlatform.Services.Course;
using StudyPlatform.Web.View.Models.Course;

namespace StudyPlatform.Tests
{
    [TestFixture]
    public class CourseViewFormServiceTests
    {
        private StudyPlatformDbContext dbContext;
        private Mock<CourseViewFormService> _courseViewFormServiceMock;
        private Mock<CourseViewService> _courseViewServiceMock;
        private Mock<CategoryViewService> _categoryViewServiceMock;
        private int defaultCourseId = 1;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ICollection<Data.Models.Category> categoryData = new List<Data.Models.Category>()
            {
                new Data.Models.Category{Id = 1, Name = "In Memory Category 1"},
                new Data.Models.Category{Id = 2, Name = "In Memory Category 2"}
            };

            //Arrange
            ICollection<Data.Models.Course> courseData = new List<Data.Models.Course>()
            {
                new Data.Models.Course{Id = 1, Name = "In Memory Course 1", Description = "In Memory Course 1 Description", CategoryId = 1},
                new Data.Models.Course{Id = 2, Name = "In Memory Course 2", Description = "In Memory Course 2 Description", CategoryId = 1}
            };

            var dbOptions = new DbContextOptionsBuilder<StudyPlatformDbContext>()
                .UseInMemoryDatabase(databaseName: "CourseViewFormServiceTests_InMemory")
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
            _courseViewFormServiceMock = new Mock<CourseViewFormService>(this.dbContext);
        }

        [Test]
        public async Task CourseViewForm_AddAsync_ShouldAddNewCourseInDb()
        {
            int coursesCount = this.dbContext.Courses.Count();
            var formModel = await this._courseViewServiceMock.Object.GetFormCourseAsync(defaultCourseId);
            formModel.Id = 3;
            formModel.Name = "In Memory Course 3";
            formModel.Description = "In Memory Course 3 Description";

            await this._courseViewFormServiceMock.Object.AddAsync(formModel);

            bool anyResult = await this._courseViewServiceMock.Object.AnyByIdAsync(formModel.Id);

            Assert.True(anyResult);
            Assert.That(coursesCount + 1, Is.EqualTo(3));
        }

        [Test]
        public async Task CourseViewForm_AssignToNewCategoryAsync_ShouldChangeCategoryIdOfCourseObjectIfCategoryIdIsValid()
        {
            int newCategoryId = 2;

            await this._courseViewFormServiceMock.Object.AssignToNewCategoryAsync(defaultCourseId, newCategoryId);
            var modelToTest = await this._courseViewServiceMock.Object.GetById(defaultCourseId);

            Assert.AreEqual(newCategoryId, modelToTest.CategoryId);
        }

        [Test]
        public async Task CourseViewForm_EditAsync_ShouldEditExistingCourseEntryIfDataIsValid()
        {
            var oldCourseModel = await this._courseViewServiceMock.Object.GetById(defaultCourseId);

            string newName = "In Memory Course 1 EDITED";
            string newDescription = "In Memory Course 1 Description EDITED";
            int newCategoryId = 1;

            CourseViewFormModel modelToEdit = new CourseViewFormModel()
            {
                Id = defaultCourseId,
                Name = newName,
                Description = newDescription,
                CategoryId = newCategoryId
            };

            await this._courseViewFormServiceMock.Object.EditAsync(modelToEdit);
            var newCourseModel = await this._courseViewServiceMock.Object.GetById(defaultCourseId);

            Assert.AreEqual(newCourseModel.Id, oldCourseModel.Id);
            Assert.AreNotEqual(newCourseModel.Name, oldCourseModel.Name);
            Assert.AreNotEqual(newCourseModel.Description, oldCourseModel.Description);
            Assert.AreNotEqual(newCourseModel.CategoryId, oldCourseModel.CategoryId);
        }

        [Test]
        public async Task CourseViewForm_RemoveAsync_ShouldRemoveCourseEntityIfIdExists()
        {
            bool courseStillExists = await this._courseViewServiceMock.Object.AnyByIdAsync(defaultCourseId);
            Assert.True(courseStillExists);

            await this._courseViewFormServiceMock.Object.RemoveAsync(defaultCourseId);

            bool courseDoesntExist = await this._courseViewServiceMock.Object.AnyByIdAsync(defaultCourseId);
            Assert.False(courseDoesntExist);
        }

        [Test]
        public async Task CourseViewForm_RemoveAsync_ShouldThrowErrorIfIdIsInvalid()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () => await this._courseViewFormServiceMock.Object.RemoveAsync(-1));
        }
    }
}
