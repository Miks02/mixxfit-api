using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedDataForWorkoutTemplatedAndTemplateExercises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WorkoutTemplates",
                columns: new[] { "Id", "FitnessProfileId", "Name", "Notes" },
                values: new object[,]
                {
                    { 1, null, "Full Body Strength A", "Balanced full-body strength session focused on compound lifts with moderate volume and steady progression." },
                    { 2, null, "Full Body Strength B", "Alternate full-body day emphasizing posterior chain work, squat variation, and upper body strength." },
                    { 3, null, "Push Hypertrophy", "Upper-body push session built for chest, shoulders, and triceps hypertrophy with controlled tempo." },
                    { 4, null, "Pull Hypertrophy", "Upper-body pull session targeting back thickness, hinge strength, and biceps development." },
                    { 5, null, "Legs and Core", "Lower-body and core session centered on squat strength, posterior chain support, and trunk stability." },
                    { 6, null, "Posterior Chain Focus", "Hinge-dominant training day focused on glutes, hamstrings, and lower back resilience." }
                });

            migrationBuilder.InsertData(
                table: "WorkoutTemplateExercises",
                columns: new[] { "ExerciseId", "WorkoutTemplateId", "Order", "SetCount" },
                values: new object[,]
                {
                    { 1, 1, 2, 4 },
                    { 7, 1, 3, 4 },
                    { 13, 1, 4, 3 },
                    { 25, 1, 1, 5 },
                    { 28, 1, 5, 3 },
                    { 34, 1, 6, 2 },
                    { 2, 2, 2, 4 },
                    { 8, 2, 3, 4 },
                    { 10, 2, 1, 5 },
                    { 18, 2, 6, 2 },
                    { 26, 2, 4, 3 },
                    { 36, 2, 5, 3 },
                    { 1, 3, 1, 4 },
                    { 2, 3, 2, 4 },
                    { 4, 3, 3, 3 },
                    { 13, 3, 4, 3 },
                    { 16, 3, 5, 2 },
                    { 21, 3, 6, 3 },
                    { 7, 4, 1, 4 },
                    { 8, 4, 2, 4 },
                    { 9, 4, 3, 3 },
                    { 10, 4, 4, 4 },
                    { 11, 4, 5, 3 },
                    { 23, 4, 6, 2 },
                    { 25, 5, 1, 4 },
                    { 26, 5, 2, 3 },
                    { 27, 5, 3, 3 },
                    { 28, 5, 4, 4 },
                    { 30, 5, 5, 3 },
                    { 34, 5, 6, 2 },
                    { 35, 5, 7, 2 },
                    { 10, 6, 1, 5 },
                    { 11, 6, 2, 4 },
                    { 28, 6, 3, 4 },
                    { 31, 6, 4, 3 },
                    { 36, 6, 5, 3 },
                    { 37, 6, 6, 2 },
                    { 38, 6, 7, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 7, 1 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 13, 1 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 25, 1 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 28, 1 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 34, 1 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 8, 2 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 10, 2 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 18, 2 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 26, 2 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 36, 2 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 13, 3 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 16, 3 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 21, 3 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 7, 4 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 8, 4 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 9, 4 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 10, 4 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 11, 4 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 23, 4 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 25, 5 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 26, 5 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 27, 5 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 28, 5 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 30, 5 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 34, 5 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 35, 5 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 10, 6 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 11, 6 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 28, 6 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 31, 6 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 36, 6 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 37, 6 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplateExercises",
                keyColumns: new[] { "ExerciseId", "WorkoutTemplateId" },
                keyValues: new object[] { 38, 6 });

            migrationBuilder.DeleteData(
                table: "WorkoutTemplates",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WorkoutTemplates",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WorkoutTemplates",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WorkoutTemplates",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "WorkoutTemplates",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "WorkoutTemplates",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
