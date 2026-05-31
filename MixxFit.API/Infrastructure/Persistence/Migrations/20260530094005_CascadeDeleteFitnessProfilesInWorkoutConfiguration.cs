using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteFitnessProfilesInWorkoutConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_FitnessProfiles_FitnessProfileId",
                table: "Workouts");

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_FitnessProfiles_FitnessProfileId",
                table: "Workouts",
                column: "FitnessProfileId",
                principalTable: "FitnessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_FitnessProfiles_FitnessProfileId",
                table: "Workouts");

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_FitnessProfiles_FitnessProfileId",
                table: "Workouts",
                column: "FitnessProfileId",
                principalTable: "FitnessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
