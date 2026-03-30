using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeExerciseNonNullableInExerciseEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseEntries_Exercises_ExerciseId",
                table: "ExerciseEntries");

            migrationBuilder.AlterColumn<int>(
                name: "ExerciseId",
                table: "ExerciseEntries",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseEntries_Exercises_ExerciseId",
                table: "ExerciseEntries",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseEntries_Exercises_ExerciseId",
                table: "ExerciseEntries");

            migrationBuilder.AlterColumn<int>(
                name: "ExerciseId",
                table: "ExerciseEntries",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseEntries_Exercises_ExerciseId",
                table: "ExerciseEntries",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id");
        }
    }
}
