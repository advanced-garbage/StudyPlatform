using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Moq;
using StudyPlatform.Data;
using StudyPlatform.Data.Models;
using StudyPlatform.Services.Roles;
using System.Security.Claims;
using System.Security.Principal;
using static StudyPlatform.Common.GeneralConstants;
using static System.Formats.Asn1.AsnWriter;

namespace StudyPlatform.Tests
{
    [TestFixture]
    public class RoleServiceTests
    {
        private Mock<RoleService> _roleServiceMock;
        private Mock<IHttpContextAccessor> _contextAccessorMock;
        private Mock<IUserStore<ApplicationUser>> _userStoreMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<ApplicationUser> mockUser;

        private Guid defaultGuid;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            defaultGuid = Guid.NewGuid();
        }

        [SetUp]
        public async Task SetUp()
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Mock User Username"),
                new Claim(ClaimTypes.NameIdentifier, "Mock User Id")
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "testAuthType");
            var mockUserClaimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            mockUser = new Mock<ApplicationUser>();
            mockUser.Setup(u => u.Id).Returns(defaultGuid); 
            mockUser.Setup(u => u.UserName).Returns("Mock User Username");

            this._contextAccessorMock = new Mock<IHttpContextAccessor>();
            this._contextAccessorMock.Setup(c => c.HttpContext.User).Returns(mockUserClaimsPrincipal);

            this._userStoreMock = new Mock<IUserStore<ApplicationUser>>();

            this._userManagerMock = new Mock<UserManager<ApplicationUser>>(_userStoreMock.Object, null, null, null, null, null, null, null, null); ;
            this._userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                                 .ReturnsAsync(mockUser.Object);

            this._roleServiceMock = new Mock<RoleService>(this._userManagerMock.Object,
                                                          this._contextAccessorMock.Object);
        }

        [Test]
        public async Task RoleService_GetRoleNameAsync_ShouldReturnUserStringIfUserIsOfRoleUser()
        {
            string titleToTest = UserRoleName;
            this._userManagerMock.Setup(u => u.IsInRoleAsync(It.IsAny<ApplicationUser>(), titleToTest))
                                 .ReturnsAsync(true);
            string result = await this._roleServiceMock.Object.GetRoleNameAsync();

            Assert.AreEqual(titleToTest, result);
        }

        [Test]
        public async Task RoleService_GetRoleNameAsync_ShouldReturnTeacherStringIfUserIsOfRoleTeacher()
        {
            string titleToTest = TeacherRoleName;
            this._userManagerMock.Setup(u => u.IsInRoleAsync(It.IsAny<ApplicationUser>(), titleToTest))
                                 .ReturnsAsync(true);
            string result = await this._roleServiceMock.Object.GetRoleNameAsync();

            Assert.AreEqual(titleToTest, result);
        }

        [Test]
        public async Task RoleService_GetRoleNameAsync_ShouldReturnAdministratorStringIfUserIsOfRoleAdministrator()
        {
            string titleToTest = AdministratorRoleName;
            this._userManagerMock.Setup(u => u.IsInRoleAsync(It.IsAny<ApplicationUser>(), titleToTest))
                                 .ReturnsAsync(true);
            string result = await this._roleServiceMock.Object.GetRoleNameAsync();

            Assert.AreEqual(titleToTest, result);
        }

        [Test]
        public async Task RoleService_IsTeacherRole_ShouldReturnTrueIfUserIsInRoleTeacher()
        {
            string titleToTest = TeacherRoleName;
            this._userManagerMock.Setup(u => u.IsInRoleAsync(It.IsAny<ApplicationUser>(), titleToTest))
                                 .ReturnsAsync(true);
            bool result = await this._roleServiceMock.Object.IsTeacherRole();

            Assert.True(result);
        }

        [Test]
        public async Task RoleService_IsTeacherRole_ShouldReturnFalseIfUserIsNotInRoleTeacher()
        {
            string titleToTest = UserRoleName;
            this._userManagerMock.Setup(u => u.IsInRoleAsync(It.IsAny<ApplicationUser>(), titleToTest))
                                 .ReturnsAsync(true);
            bool result = await this._roleServiceMock.Object.IsTeacherRole();

            Assert.False(result);
        }
    }
}
