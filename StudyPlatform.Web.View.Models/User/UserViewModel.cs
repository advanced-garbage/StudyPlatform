using StudyPlatform.Web.View.Models.Lesson;
using System;
using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.Users;

namespace StudyPlatform.Web.View.Models.User
{
    public class UserViewModel : IValidatableObject
    {
        public UserViewModel()
        {
        }
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
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // age check
            if (!int.TryParse(Age, out int age) && (age <= 8 || age >= 125))
            {
                yield return new ValidationResult("Age either isn't given as a number or is out of bounds (8-125)");
            }
        }

    }
}
