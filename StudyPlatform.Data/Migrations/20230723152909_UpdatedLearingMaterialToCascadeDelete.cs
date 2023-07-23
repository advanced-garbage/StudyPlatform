using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyPlatform.Data.Migrations
{
    public partial class UpdatedLearingMaterialToCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningMaterials_Lessons_LessonId",
                table: "LearningMaterials");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningMaterials_Lessons_LessonId",
                table: "LearningMaterials",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningMaterials_Lessons_LessonId",
                table: "LearningMaterials");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningMaterials_Lessons_LessonId",
                table: "LearningMaterials",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
