using Microsoft.EntityFrameworkCore;
using Moq;
using StudyPlatform.Data;
using StudyPlatform.Services.Category;


namespace StudyPlatform.Tests
{
    [TestFixture]
    public class CategoryViewServiceTests
    {
        private Mock<CategoryViewService> _categoryViewServiceMock;
        private StudyPlatformDbContext dbContext;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            //Arrange
            ICollection<Data.Models.Category> categoryData = new List<Data.Models.Category>()
            {
                new Data.Models.Category{Id = 1, Name = "In Memory Category 1"},
                new Data.Models.Category{Id = 2, Name = "In Memory Category 2"}
            };

            var dbOptions = new DbContextOptionsBuilder<StudyPlatformDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoryViewServiceTests_InMemory")
                .Options;

            this.dbContext = new StudyPlatformDbContext(dbOptions);
            this.dbContext.Categories.AddRange(categoryData);
            this.dbContext.SaveChanges();
        }

        [SetUp]
        public void Setup()
        { 
            _categoryViewServiceMock = new Mock<CategoryViewService>(this.dbContext);
        }

        [Test]
        public async Task CategoryAllService_GetCategoryByIdAsync_ShouldReturnById()
        {
            int categoryId = 1;
            // act
            var result = await _categoryViewServiceMock.Object.GetCategoryByIdAsync(categoryId);
            
            // assert
            Assert.AreEqual(categoryId, result.Id, "Didn't return a category view model with the same id.");
        }

        [Test]
        public async Task CategoryAllService_GetCategoryById_ShouldThrowExceptionIfIdIsInvalid()
        {
            int categoryId = -1;
            // act
            //var result = await _categoryViewServiceMock.Object.GetCategoryByIdAsync(categoryId);

            // assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _categoryViewServiceMock.Object.GetCategoryByIdAsync(categoryId));
        }

        [Test]
        public async Task CategoryAllService_GetCategoryIdByCourseIdAsync_ShouldThrowErrorIfIdIsInvalid()
        {
            int courseId = -1;

            // assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _categoryViewServiceMock.Object.GetCategoryIdByCourseIdAsync(courseId));
        }

        [Test]
        public async Task CategoryAllService_GetNameByIdAsync_ShouldThrowErrorIfIdIsInvalid()
        {
            int categoryId = -1;

            // assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _categoryViewServiceMock.Object.GetNameByIdAsync(categoryId));
        }

        [Test]
        public async Task CategoryAllService_GetNameUrlByIdAsync_ShouldThrowErrorIfIdIsInvalid()
        {
            int categoryId = -1;

            // assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _categoryViewServiceMock.Object.GetNameUrlByIdAsync(categoryId));
        }


        [Test]
        public async Task CategoryAllService_GetNameByIdAsync_ShouldReturnName()
        {
            string name = "In Memory Category 1";
            int categoryId = 1;
            // act
            string result = await _categoryViewServiceMock.Object.GetNameByIdAsync(categoryId);

            // assert
            Assert.NotNull(result);
            Assert.That(result, Does.Contain(name));
        }

        [Test]
        public async Task CategoryAllService_AnyByIdAsync_ShouldReturnTrue()
        {
            int categoryId = 1;
            // act
            bool result = await _categoryViewServiceMock.Object.AnyByIdAsync(categoryId);

            // assert
            Assert.True(result);
        }

        [Test]
        public async Task CategoryAllService_AnyByIdAsync_ShouldReturnFalse()
        {
            int categoryId = -1;
            // act
            bool result = await _categoryViewServiceMock.Object.AnyByIdAsync(categoryId);

            // assert
            Assert.False(result);
        }

        [Test]
        public async Task CategoryAllService_AnyByNameAsync_ShouldReturnTrue()
        {
            string name = "In Memory Category 1";
            // act
            bool result = await _categoryViewServiceMock.Object.AnyByNameAsync(name);

            // assert
            Assert.True(result);
        }

        [Test]
        public async Task CategoryAllService_AnyByNameAsync_ShouldReturnFalse()
        {
            string name = "In Memory Category -1";
            // act
            bool result = await _categoryViewServiceMock.Object.AnyByNameAsync(name);

            // assert
            Assert.False(result);
        }

        [Test]
        public async Task CategoryAllService_GetAllCategoriesAsync_ShouldReturnAtLeast2()
        {
            // act
            var result = await _categoryViewServiceMock.Object.GetAllCategoriesAsync();

            // assert
            Assert.NotNull(result);
            Assert.That(result.Count >= 2);
        }

        [Test]
        public async Task CategoryAllService_GetCategoriesForAllPageAsync_AllCategoriesShouldHaveCategoriesWithCountOfAtLeast2()
        {
            // act
            var result = await _categoryViewServiceMock.Object.GetCategoriesForAllPageAsync();
            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Categories);
            Assert.True(result.Categories.Count() >= 2);
        }

        [Test]
        public async Task CategoryAllService_GetFormCategory_ShouldReturnFormById()
        {
            int categoryId = 1;
            // act
            var result = await _categoryViewServiceMock.Object.GetFormCategory(categoryId);
            // assert
            Assert.NotNull(result, "Form model is not null");
            Assert.That(categoryId == result.Id, "Form model has the same input as the param sent");
        }

        [Test]
        public async Task CategoryAllService_GetNameUrlByIdAsync_ShouldReturnNameUrlById()
        {
            int categoryId = 1;
            // act
            string result = await _categoryViewServiceMock.Object.GetNameUrlByIdAsync(categoryId);
            // assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task CategoryAllService_GetNameUrlByIdAsync_NameUrlShouldHaveNoEmptySpaces()
        {
            int categoryId = 1;
            // act
            string result = await _categoryViewServiceMock.Object.GetNameUrlByIdAsync(categoryId);
            // assert
            Assert.That(!result.Contains(" "));
        }

    }
}