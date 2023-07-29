using Microsoft.EntityFrameworkCore;
using Moq;
using StudyPlatform.Data;
using StudyPlatform.Services.Category;
using StudyPlatform.Web.View.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Tests
{
    [TestFixture]
    public class CategoryViewFormServiceTests
    {
        private StudyPlatformDbContext dbContext;
        private Mock<CategoryViewService> _categoryViewServiceMock;
        private Mock<CategoryViewFormService> _categoryViewFormServiceMock;
        ICollection<Data.Models.Category> categoryData;
        private int categoriesCount;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            categoryData = new List<Data.Models.Category>()
            {
                new Data.Models.Category{Id = 1, Name = "In Memory Category 1"},
                new Data.Models.Category{Id = 2, Name = "In Memory Category 2"}
            };

            var dbOptions = new DbContextOptionsBuilder<StudyPlatformDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoryViewFormServiceTests_InMemory")
                .Options;
            this.dbContext = new StudyPlatformDbContext(dbOptions);
            this.dbContext.Categories.AddRange(categoryData);
            this.dbContext.SaveChanges();
        }

        [SetUp]
        public void SetUp()
        {
            _categoryViewServiceMock = new Mock<CategoryViewService>(this.dbContext);
            _categoryViewFormServiceMock = new Mock<CategoryViewFormService>(this.dbContext);
        }

        [Test]
        public async Task CategoryFormTest_AddAsync_ShouldIncreaseCountOfCategoriesInDb()
        {
            // arrange
            categoriesCount = this.dbContext.Categories.Count();
            CategoryViewFormModel testModel = new CategoryViewFormModel()
            {
                Id = 3,
                Name = "In Memory Category 3"
            };
            // act
            await _categoryViewFormServiceMock.Object.AddAsync(testModel);
            // arrange
            Assert.That(categoriesCount + 1, Is.EqualTo(this.dbContext.Categories.Count()));
        }

        [Test]
        public async Task CategoryFormTest_EditAsync_ShouldChangeDataFromExistingEntity()
        {
            // arrange
            int categoryId = 3;
            string newName = "In Memory Category 3 EDITED";
            var testModel = await this._categoryViewServiceMock.Object.GetFormCategory(categoryId);
            testModel.Name = newName;

            // act
            await this._categoryViewFormServiceMock.Object.EditAsync(testModel);

            // assert
            string assertName = await this._categoryViewServiceMock.Object.GetNameByIdAsync(categoryId);
            Assert.That(assertName == newName);
        }

        [Test]
        public async Task CategoryFormTest_RemoveAsync_ShouldDecreaseCountOfCategoriesInDb()
        {
            // arrange
            categoriesCount = this.dbContext.Categories.Count();
            int categoryId = 3;

            // act
            await this._categoryViewFormServiceMock.Object.RemoveAsync(categoryId);

            // assert
            Assert.False(await this._categoryViewServiceMock.Object.AnyByIdAsync(categoryId));
            Assert.That(categoriesCount - 1, Is.EqualTo(this.dbContext.Categories.Count()));
        }

        [Test]
        public async Task CategoryFormTest_RemoveAsync_ShouldThrowErrorIfIdIsInvalid()
        {
            // arrange
            int categoryId = -1;

            // act & assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await this._categoryViewFormServiceMock.Object.RemoveAsync(categoryId));
        }
    }
}
