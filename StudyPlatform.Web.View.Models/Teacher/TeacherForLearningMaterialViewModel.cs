using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using static StudyPlatform.Common.ModelValidationConstants.Users;

namespace StudyPlatform.Web.View.Models.Teacher
{
    public class TeacherForLearningMaterialViewModel
    {
        public Guid Id { get; set; }

        [StringLength(FirstNameMaxLength, MinimumLength = LastNameMinLength)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(UserNameMinLength, MinimumLength = UserNameMinLength)]
        public string UserName { get; set; }
    }
}
