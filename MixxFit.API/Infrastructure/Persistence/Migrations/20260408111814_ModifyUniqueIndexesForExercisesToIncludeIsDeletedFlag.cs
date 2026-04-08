using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ModifyUniqueIndexesForExercisesToIncludeIsDeletedFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Exercises_Name_ExerciseCategoryId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_Name_UserId",
                table: "Exercises");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_Name_ExerciseCategoryId",
                table: "Exercises",
                columns: new[] { "Name", "ExerciseCategoryId" },
                unique: true,
                filter: "\"UserId\" IS NULL AND \"IsDeleted\" = FALSE");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_Name_UserId",
                table: "Exercises",
                columns: new[] { "Name", "UserId" },
                unique: true,
                filter: "\"IsDeleted\" = FALSE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Exercises_Name_ExerciseCategoryId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_Name_UserId",
                table: "Exercises");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_Name_ExerciseCategoryId",
                table: "Exercises",
                columns: new[] { "Name", "ExerciseCategoryId" },
                unique: true,
                filter: "\"UserId\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_Name_UserId",
                table: "Exercises",
                columns: new[] { "Name", "UserId" },
                unique: true);
        }
    }
}
