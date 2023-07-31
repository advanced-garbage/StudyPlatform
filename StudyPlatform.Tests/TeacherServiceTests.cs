using Microsoft.EntityFrameworkCore;
using Moq;
using StudyPlatform.Data.Models;
using StudyPlatform.Data;
using StudyPlatform.Services.TeacherLesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyPlatform.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using static StudyPlatform.Common.GeneralConstants;

namespace StudyPlatform.Tests
{
    [TestFixture]
    public class TeacherServiceTests
    {
        private StudyPlatformDbContext _dbContext;
        private Mock<TeacherService> _teacherServiceMock;
        private Mock<TeacherFormService> _teacherFormServiceMock;

        private Mock<IHttpContextAccessor> _contextAccessorMock;
        private Mock<IUserStore<ApplicationUser>> _userStoreMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<RoleManager<IdentityRole<Guid>>> _roleManagerMock;

        private ApplicationUser _fakeUser;

        private int defaultLessonId = 1;
        private Guid defaultGuid;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            //Arrange
            defaultGuid = Guid.NewGuid();

            SeedUser();

            ICollection<Teacher> teacherData = new List<Teacher>()
            {
                new Teacher{ Id = defaultGuid }
            };

            var dbOptions = new DbContextOptionsBuilder<StudyPlatformDbContext>()
                .UseInMemoryDatabase(databaseName: "TeacherServiceTests_InMemory")
                .Options;
            this._dbContext = new StudyPlatformDbContext(dbOptions);
            this._dbContext.Users.AddRange(_fakeUser);
            this._dbContext.Teachers.AddRange(teacherData);
            this._dbContext.SaveChanges();
            this._dbContext.TeacherLessons.Add(new TeacherLesson { TeacherId = defaultGuid, LessonId = defaultLessonId });
            this._dbContext.SaveChanges();

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Fake Username"),
                new Claim(ClaimTypes.NameIdentifier, "Fake User Id")
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "testAuthType");
            var mockUserClaimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            this._contextAccessorMock = new Mock<IHttpContextAccessor>();
            this._contextAccessorMock.Setup(c => c.HttpContext.User).Returns(mockUserClaimsPrincipal);

            this._roleManagerMock = new Mock<RoleManager<IdentityRole<Guid>>>(new Mock<IRoleStore<IdentityRole<Guid>>>().Object,
                                                                                new IRoleValidator<IdentityRole<Guid>>[0],
                                                                                new Mock<ILookupNormalizer>().Object,
                                                                                new Mock<IdentityErrorDescriber>().Object,
                                                                                new Mock<ILogger<RoleManager<IdentityRole<Guid>>>>().Object);
            this._roleManagerMock.Setup(r => r.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
        }

        [SetUp]
        public void Setup()
        {
            this._userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var _mockUserRoleStore = this._userStoreMock.As<IUserRoleStore<IdentityUser>>();
            this._userManagerMock = new Mock<UserManager<ApplicationUser>>(_mockUserRoleStore.Object, null, null, null, null, null, null, null, null); ;
            this._userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                                 .ReturnsAsync(_fakeUser);

            this._teacherFormServiceMock = new Mock<TeacherFormService>(this._dbContext,
                                                                        this._roleManagerMock.Object,
                                                                        this._userManagerMock.Object,
                                                                        this._contextAccessorMock.Object);
            this._teacherServiceMock = new Mock<TeacherService>(this._dbContext);
        }

        [Test]
        public async Task TeacherService_AnyById_ShouldReturnTrueIfIdIsValid()
        {
            bool result = await this._teacherServiceMock.Object.AnyById(defaultGuid);

            Assert.True(result);
        }
        [Test]
        public async Task TeacherService_AnyById_ShouldReturnFalseIfIdIsInvalid()
        {
            bool result = await this._teacherServiceMock.Object.AnyById(Guid.Empty);

            Assert.False(result);
        }

        [Test]
        public async Task TeacherFormService_AddTeacher_ShouldAddTeacherEntityToDb()
        {
            int teachersCount = this._dbContext.Teachers.Count();
            Guid newTeacherId = Guid.NewGuid();

            await this._teacherFormServiceMock.Object.AddTeacherAsync(newTeacherId);
            bool IdExists = await this._dbContext.Teachers.AnyAsync(t => t.Id.Equals(newTeacherId));

            Assert.That(this._dbContext.Teachers.Count, Is.EqualTo(teachersCount + 1));
            Assert.True(IdExists);
        }


        private void SeedUser()
        {
            this._fakeUser = new ApplicationUser();
            this._fakeUser.Id = defaultGuid;
            this._fakeUser.UserName = "Fake Username";
            this._fakeUser.FirstName = "Fake First Name";
            this._fakeUser.MiddleName = "Fake Middle Name";
            this._fakeUser.LastName = "Fake Last Name";
            this._fakeUser.Age = "50";
            this._fakeUser.Email = "fakeuser@test.com";
        }
    }
}
