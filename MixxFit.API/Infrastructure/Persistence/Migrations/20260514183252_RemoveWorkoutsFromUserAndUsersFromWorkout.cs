using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWorkoutsFromUserAndUsersFromWorkout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_FitnessProfiles_FitnessProfileId",
                table: "Workouts");

            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_Users_UserId",
                table: "Workouts");

            migrationBuilder.DropIndex(
                name: "IX_Workouts_UserId",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Workouts");

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_FitnessProfiles_FitnessProfileId",
                table: "Workouts",
                column: "FitnessProfileId",
                principalTable: "FitnessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_FitnessProfiles_FitnessProfileId",
                table: "Workouts");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Workouts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_UserId",
                table: "Workouts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_FitnessProfiles_FitnessProfileId",
                table: "Workouts",
                column: "FitnessProfileId",
                principalTable: "FitnessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_Users_UserId",
                table: "Workouts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
