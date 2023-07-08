using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Web.View.Models.Teacher
{
    public class TeacherFormModel : IValidatableObject
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

            //if (ProfilePicture.Length > 2048)
            //{
            //    yield return new ValidationResult("URL exceeded the length limit (2048)");
            //}
        }
    }
}
