using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using static StudyPlatform.Common.ModelValidationConstants.Users;

namespace StudyPlatform.Web.View.Models.Teacher
{
    public class TeacherForLessonModel 
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        public string FirstName { get; set; }

        [StringLength(MiddleNameMaxLength, MinimumLength = MiddleNameMinLength)]
        public string? MiddleName { get; set; }

        [Required]  
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        public string LastName { get; set; }

        [Required]
        [StringLength(UserNameMaxLength, MinimumLength = UserNameMinLength)]
        public string UserName { get; set; }

    }
}
