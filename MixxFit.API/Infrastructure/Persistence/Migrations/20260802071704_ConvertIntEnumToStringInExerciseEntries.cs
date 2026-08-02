using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConvertIntEnumToStringInExerciseEntries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExerciseTypeTemp",
                table: "ExerciseEntries",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                UPDATE ""ExerciseEntries""
                SET ""ExerciseTypeTemp"" = CASE 
                    WHEN ""ExerciseType"" = 1 THEN 'WeightLifting'
                    WHEN ""ExerciseType"" = 2 THEN 'BodyWeight'
                    WHEN ""ExerciseType"" = 3 THEN 'Cardio'
                    WHEN ""ExerciseType"" = 4 THEN 'Stretching'
                    ELSE 'Other'
                END
            ");

            migrationBuilder.AlterColumn<string>(
                name: "ExerciseType",
                table: "ExerciseEntries",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
            
            migrationBuilder.Sql(@"
                UPDATE ""ExerciseEntries""
                SET ""ExerciseType"" = ""ExerciseTypeTemp""
            ");

            migrationBuilder.DropColumn(
                name: "ExerciseTypeTemp",
                table: "ExerciseEntries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ExerciseType",
                table: "ExerciseEntries",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
