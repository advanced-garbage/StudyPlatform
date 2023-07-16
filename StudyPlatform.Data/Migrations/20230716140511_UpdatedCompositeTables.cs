using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyPlatform.Data.Migrations
{
    public partial class UpdatedCompositeTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentLessons_Lessons_LessonId",
                table: "StudentLessons");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherLessons_Lessons_LessonId",
                table: "TeacherLessons");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentLessons_LearningMaterials_LessonId",
                table: "StudentLessons",
                column: "LessonId",
                principalTable: "LearningMaterials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherLessons_LearningMaterials_LessonId",
                table: "TeacherLessons",
                column: "LessonId",
                principalTable: "LearningMaterials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentLessons_LearningMaterials_LessonId",
                table: "StudentLessons");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherLessons_LearningMaterials_LessonId",
                table: "TeacherLessons");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentLessons_Lessons_LessonId",
                table: "StudentLessons",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherLessons_Lessons_LessonId",
                table: "TeacherLessons",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
