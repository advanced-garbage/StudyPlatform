using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyPlatform.Data.Migrations
{
    public partial class UpdatedLearningMaterialTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "LearningMaterials",
                newName: "FileName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "LearningMaterials",
                newName: "Name");
        }
    }
}
