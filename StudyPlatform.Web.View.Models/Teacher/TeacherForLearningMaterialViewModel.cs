using System.ComponentModel.DataAnnotations;
using static StudyPlatform.Common.ModelValidationConstants.Users;

namespace StudyPlatform.Web.View.Models.Teacher
{
    public class TeacherForLearningMaterialViewModel
    {
        public Guid Id { get; set; }

        [StringLength(FirstNameMaxLength, MinimumLength = LastNameMinLength)]
        public string FirstName { get; set; }

        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        public string LastName { get; set; }

        public string UserName { get; set; }
    }
}
