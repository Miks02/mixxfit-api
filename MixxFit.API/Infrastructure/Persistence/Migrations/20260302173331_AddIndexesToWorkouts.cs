using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesToWorkouts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Workouts_CreatedAt",
                table: "Workouts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_WorkoutDate",
                table: "Workouts",
                column: "WorkoutDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Workouts_CreatedAt",
                table: "Workouts");

            migrationBuilder.DropIndex(
                name: "IX_Workouts_WorkoutDate",
                table: "Workouts");
        }
    }
}
