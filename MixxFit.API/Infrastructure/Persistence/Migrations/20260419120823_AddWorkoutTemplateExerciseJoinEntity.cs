using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkoutTemplateExerciseJoinEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutTemplateExercises_Exercises_ExercisesId",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutTemplateExercises_WorkoutTemplates_WorkoutTemplatesId",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutTemplateExercises",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutTemplateExercises_WorkoutTemplatesId",
                table: "WorkoutTemplateExercises");

            migrationBuilder.RenameColumn(
                name: "WorkoutTemplatesId",
                table: "WorkoutTemplateExercises",
                newName: "SetCount");

            migrationBuilder.RenameColumn(
                name: "ExercisesId",
                table: "WorkoutTemplateExercises",
                newName: "Order");

            migrationBuilder.AddColumn<int>(
                name: "WorkoutTemplateId",
                table: "WorkoutTemplateExercises",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExerciseId",
                table: "WorkoutTemplateExercises",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutTemplateExercises",
                table: "WorkoutTemplateExercises",
                columns: new[] { "WorkoutTemplateId", "ExerciseId" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateExercises_ExerciseId",
                table: "WorkoutTemplateExercises",
                column: "ExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutTemplateExercises_Exercises_ExerciseId",
                table: "WorkoutTemplateExercises",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutTemplateExercises_WorkoutTemplates_WorkoutTemplateId",
                table: "WorkoutTemplateExercises",
                column: "WorkoutTemplateId",
                principalTable: "WorkoutTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutTemplateExercises_Exercises_ExerciseId",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutTemplateExercises_WorkoutTemplates_WorkoutTemplateId",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutTemplateExercises",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutTemplateExercises_ExerciseId",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropColumn(
                name: "WorkoutTemplateId",
                table: "WorkoutTemplateExercises");

            migrationBuilder.DropColumn(
                name: "ExerciseId",
                table: "WorkoutTemplateExercises");

            migrationBuilder.RenameColumn(
                name: "SetCount",
                table: "WorkoutTemplateExercises",
                newName: "WorkoutTemplatesId");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "WorkoutTemplateExercises",
                newName: "ExercisesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutTemplateExercises",
                table: "WorkoutTemplateExercises",
                columns: new[] { "ExercisesId", "WorkoutTemplatesId" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateExercises_WorkoutTemplatesId",
                table: "WorkoutTemplateExercises",
                column: "WorkoutTemplatesId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutTemplateExercises_Exercises_ExercisesId",
                table: "WorkoutTemplateExercises",
                column: "ExercisesId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutTemplateExercises_WorkoutTemplates_WorkoutTemplatesId",
                table: "WorkoutTemplateExercises",
                column: "WorkoutTemplatesId",
                principalTable: "WorkoutTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
