using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using StudyPlatform.Data;
using StudyPlatform.Data.Models;
using StudyPlatform.Services.Roles;
using StudyPlatform.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StudyPlatform.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<UserService> _userServiceMock;
        private StudyPlatformDbContext _dbContext;
        private ApplicationUser _fakeUser;

        private Guid defaultGuid;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _fakeUser = new ApplicationUser();
            _fakeUser.UserName = "Fake Username";
            _fakeUser.FirstName = "Fake First Name";
            _fakeUser.MiddleName = "Fake Middle Name";
            _fakeUser.LastName = "Fake Last Name";
            _fakeUser.Age = "50";
            _fakeUser.Email = "fakeuser@test.com";

            var dbOptions = new DbContextOptionsBuilder<StudyPlatformDbContext>()
                .UseInMemoryDatabase(databaseName: "UserServiceTests_InMemory")
                .Options;
            this._dbContext = new StudyPlatformDbContext(dbOptions);
            this._dbContext.Users.Add(_fakeUser);
            this._dbContext.SaveChanges();
        }

        [SetUp]
        public void SetUp()
        {
            defaultGuid = this._dbContext.Users.Where(u => u.UserName.Equals("Fake Username")).First().Id;
            this._userServiceMock = new Mock<UserService>(this._dbContext);
        }

        [Test]
        public async Task UserService_AnyById_ShouldReturnTrueIfGuidIsInDb()
        {
            bool result = await this._userServiceMock.Object.AnyById(defaultGuid);

            Assert.True(result);
        }

        [Test]
        public async Task UserService_AnyById_ShouldReturnFalseIfGuidIsInvalid()
        {
            bool result = await this._userServiceMock.Object.AnyById(Guid.Empty);

            Assert.False(result);
        }

        [Test]
        public async Task UserService_AnyByUserName_ShouldReturnTrueIfUsernameIsValid()
        {
            string userNameToTest = "Fake Username";
            bool result = await this._userServiceMock.Object.AnyByUserName(userNameToTest);

            Assert.True(result);
        }

        [Test]
        public async Task UserService_AnyByUserName_ShouldReturnFalseIfUsernameIsInvalid()
        {
            string userNameToTest = string.Empty;
            bool result = await this._userServiceMock.Object.AnyByUserName(userNameToTest);

            Assert.False(result);
        }
        [Test]
        public async Task UserService_GetUserByIdAsync_ReturnsApplicationUserIfIdIsValid()
        {
            var result = await this._userServiceMock.Object.GetUserByIdAsync(defaultGuid);

            Assert.NotNull(result);
            Assert.AreEqual(_fakeUser.UserName, result.UserName);
        }

    }
}
