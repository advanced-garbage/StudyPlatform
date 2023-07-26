using Microsoft.Extensions.Caching.Memory;
using Moq;
using StudyPlatform.Controllers;
using StudyPlatform.Services.Category;
using StudyPlatform.Services.Category.Interfaces;
using StudyPlatform.Web.View.Models.Category;
using StudyPlatform.Web.View.Models.Course;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using static StudyPlatform.Common.CacheConstants;

namespace StudyPlatform.Tests
{
    public class CategoryControllerTests
    {
        private CategoryController _categoryController;
        private Mock<ICategoryViewService> _iCategoryViewServiceMock;
        private Mock<ICategoryViewFormService> _categoryViewFormServiceMock;
        private Mock<IMemoryCache> _memoryCacheMock;

        private Mock<CategoryViewService> _categoryViewServiceMock;

        private Mock<AllCategoriesViewModel> _allCategoriesViewModelMock;
        private Mock<CategoryViewModel> _categoryViewModelMock;

        [SetUp]
        public void Setup()
        {
            _iCategoryViewServiceMock = new Mock<ICategoryViewService>();
            _categoryViewFormServiceMock = new Mock<ICategoryViewFormService>();
            _memoryCacheMock = new Mock<IMemoryCache>();

            _categoryViewServiceMock = new Mock<CategoryViewService>();
            _allCategoriesViewModelMock = new Mock<AllCategoriesViewModel>();
            _categoryViewModelMock = new Mock<CategoryViewModel>();

            //Arrange
            _categoryViewModelMock.Setup(c => c.Id).Returns(1);
            _categoryViewModelMock.Setup(c => c.Name).Returns("Mock Category");
            // _categoryViewModelMock.Setup(c => c.Courses)
            IEnumerable<CategoryViewModel> _categoryViewModelMocks
                = new List<CategoryViewModel>() {
                _categoryViewModelMock.Object
            };

            _allCategoriesViewModelMock.Setup(c => c.IsViewedByTeacher).Returns(true);
            _allCategoriesViewModelMock.Setup(c => c.Categories).Returns(_categoryViewModelMocks.AsEnumerable());

            _memoryCacheMock.Setup(c => c.Get(AllCategoriesCacheKey)).Returns(_allCategoriesViewModelMock.Object);

            _categoryController = new(_categoryViewServiceMock.Object,
                                      _categoryViewFormServiceMock.Object,
                                      _memoryCacheMock.Object);
        }

        [Test]
        public async Task CategoryAllService_ShouldReturnAll()
        {
            // act

            // assert
        }

        // WEB HOST IS STILL NOT CONFIGURED!!!
        //[Test]
        //public async Task GetAllTest()
        //{
        //    var httpClient = new HttpClient();

        //    var response = await httpClient.GetAsync("https://localhost:7289/Category/All");

        //    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        //}


    }
}