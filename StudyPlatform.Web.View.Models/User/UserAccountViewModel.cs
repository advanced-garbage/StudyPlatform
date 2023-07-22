using StudyPlatform.Web.View.Models.Lesson;
using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.Users;
using static StudyPlatform.Common.ViewModelConstants.Account;
using static StudyPlatform.Common.GeneralConstants;

namespace StudyPlatform.Web.View.Models.User
{
    public class UserAccountViewModel : IValidatableObject
    {
        [StringLength(RoleMaxLength, MinimumLength = RoleMinLength)]
        public string RoleTitle { get; set; } = string.Empty;

        [StringLength(UserNameMinLength, MinimumLength = UserNameMaxLength)]
        public string UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Display(Name = "First Name")]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        public string FirstName { get; set; }

        [StringLength(MiddleNameMaxLength, MinimumLength = MiddleNameMinLength)]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Age { get; set; }
        public AccountCreditsViewModel? Lessons;
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!int.TryParse(Age, out int age) && (age <= AgeMin || age >= AgeMax))
            {
                yield return new ValidationResult($"Age either isn't given as a number or is out of bounds ({AgeMin}-{AgeMax})");
            }
        }
    }
}
