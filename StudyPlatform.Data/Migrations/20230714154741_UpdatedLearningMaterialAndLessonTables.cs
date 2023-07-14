using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyPlatform.Data.Migrations
{
    public partial class UpdatedLearningMaterialAndLessonTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningMaterials_Lessons_LessonId",
                table: "LearningMaterials");

            migrationBuilder.RenameColumn(
                name: "LessonName",
                table: "LearningMaterials",
                newName: "LearningMaterialName");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningMaterials_Lessons_LessonId",
                table: "LearningMaterials",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningMaterials_Lessons_LessonId",
                table: "LearningMaterials");

            migrationBuilder.RenameColumn(
                name: "LearningMaterialName",
                table: "LearningMaterials",
                newName: "LessonName");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningMaterials_Lessons_LessonId",
                table: "LearningMaterials",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
