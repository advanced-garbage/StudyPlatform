using System.ComponentModel.DataAnnotations;
using System.Runtime;
using static StudyPlatform.Common.ModelValidationConstants.Users;

namespace StudyPlatform.Web.View.Models.Student
{
    public class StudentViewFormModel : IValidatableObject
    {
        public int Id { get; set; }
        public string FirstName { get; set; }   
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        [RegularExpression("@\\d")]
        public string Age { get; set; } 
        //public string ProfilePicture { get; set; }

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
