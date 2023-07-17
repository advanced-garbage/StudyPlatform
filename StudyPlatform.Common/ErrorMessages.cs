namespace StudyPlatform.Common
{
    /// <summary>
    /// Class for saving error messages.
    /// </summary>
    public static class ErrorMessages
    {
        public const string IdNotFound = "Id was not found!";

        /// <summary>
        /// Error messages for the LearningMaterial Controller and Models.
        /// </summary>
        public static class LearningMaterial
        {
            public const string FileByNameExists = "File by this name already exists.";
            public const string FileNotSent = "File was not sent";
        }

        /// <summary>
        /// Error messages for the Lesson Controller and Models.
        /// </summary>
        public static class Lesson
        {
            public const string LessonByNameExists = "Lesson by this name already exists.";
        }

        /// <summary>
        /// Error messages for the Category Controller and Models.
        /// </summary>
        public static class Category
        {
            public const string CategoryIdNotFound = "Id for this category was not found!";
            public const string CategoryByNameExists = "Object with this name already exists in the DataBase.";
        }

        /// <summary>
        /// Error messages for the Category Controller and Models.
        /// </summary>
        public static class Course
        {
            public const string CourseIdNotFound = "Id for this course was not found!";
        }
    }
}
