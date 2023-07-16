using StudyPlatform.Web.View.Models.Lesson;
using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.Users;


namespace StudyPlatform.Web.View.Models.User
{
    public class UserAccountViewModel 
    {
        // role has default value. can be admin
        [StringLength(RoleMaxLength, MinimumLength = RoleMinLength)]
        public string Role { get; set; } = "Student";
        public string UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        public string FirstName { get; set; }

        [StringLength(MiddleNameMaxLength, MinimumLength = MiddleNameMinLength)]
        public string MiddleName { get; set; }

        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        public string LastName { get; set; }

        public string Age { get; set; }

        public AccountCreditsViewModel? Lessons;
    }
}
