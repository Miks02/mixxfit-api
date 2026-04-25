using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceUserWithFitnessProfileInWorkoutTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutTemplates_Users_UserId",
                table: "WorkoutTemplates");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutTemplates_UserId_Name",
                table: "WorkoutTemplates");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WorkoutTemplates");

            migrationBuilder.DropColumn(
                name: "BMI",
                table: "FitnessProfiles");

            migrationBuilder.AddColumn<int>(
                name: "FitnessProfileId",
                table: "WorkoutTemplates",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplates_FitnessProfileId_Name",
                table: "WorkoutTemplates",
                columns: new[] { "FitnessProfileId", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutTemplates_FitnessProfiles_FitnessProfileId",
                table: "WorkoutTemplates",
                column: "FitnessProfileId",
                principalTable: "FitnessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutTemplates_FitnessProfiles_FitnessProfileId",
                table: "WorkoutTemplates");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutTemplates_FitnessProfileId_Name",
                table: "WorkoutTemplates");

            migrationBuilder.DropColumn(
                name: "FitnessProfileId",
                table: "WorkoutTemplates");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WorkoutTemplates",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "BMI",
                table: "FitnessProfiles",
                type: "double precision",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplates_UserId_Name",
                table: "WorkoutTemplates",
                columns: new[] { "UserId", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutTemplates_Users_UserId",
                table: "WorkoutTemplates",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
