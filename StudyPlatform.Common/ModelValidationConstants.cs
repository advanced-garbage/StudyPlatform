using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// <summary>
// Class for holding constants to various models.
// </summary>
namespace StudyPlatform.Common
{
    public static class ModelValidationConstants
    {
        public static class Category
        {
            public const int NameMaxLength = 40;
            public const int NameMinLength = 3;
        }

        public static class Course
        {
            public const int NameMaxLength = 64;
            public const int NameMinLength = 4;

            public const int DescriptionMaxLength = 2048;
        }

        public static class Lesson
        {
            public const int NameMaxLength = 128;
            public const int NameMinLength = 4;

            public const int DescriptionMaxLength = 2048;
        }

        public static class LearningMaterial
        {
            public const int NameMaxLength = 128;
            public const int NameMinLength = 4;
        }

        public static class Users
        {
            public const int FirstNameMaxLength = 128;
            public const int FirstNameMinLength = 4;

            public const int MiddleNameMaxLength = 128;
            public const int MiddleNameMinLength = 4;

            public const int LastNameMaxLength = 128;
            public const int LastNameMinLength = 4;

            public const int RoleMaxLength = 25;
            public const int RoleMinLength = 4;
        }
    }
}
