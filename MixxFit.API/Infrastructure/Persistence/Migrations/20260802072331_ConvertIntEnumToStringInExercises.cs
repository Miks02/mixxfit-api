using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConvertIntEnumToStringInExercises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AddColumn<string>(
                name: "ExerciseTypeTemp",
                table: "Exercises",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                UPDATE ""Exercises""
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
                table: "Exercises",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
            
            migrationBuilder.Sql(@"
                UPDATE ""Exercises""
                SET ""ExerciseType"" = ""ExerciseTypeTemp""
            ");

            migrationBuilder.DropColumn(
                name: "ExerciseTypeTemp",
                table: "Exercises");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 2,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 3,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 4,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 5,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 6,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 7,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 8,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 9,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 10,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 11,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 12,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 13,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 14,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 15,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 16,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 17,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 18,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 19,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 20,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 21,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 22,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 23,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 24,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 25,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 26,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 27,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 28,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 29,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 30,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 31,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 32,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 33,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 34,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 35,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 36,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 37,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 38,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 39,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 40,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 41,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 42,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 43,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 44,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 45,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 46,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 47,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 48,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 49,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 50,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 51,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 52,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 53,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 54,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 55,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 56,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 57,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 58,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 59,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 60,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 61,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 62,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 63,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 64,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 65,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 66,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 67,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 68,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 69,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 70,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 71,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 72,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 73,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 74,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 75,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 76,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 77,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 78,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 79,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 80,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 81,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 82,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 83,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 84,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 85,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 86,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 87,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 88,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 89,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 90,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 91,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 92,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 93,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 94,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 95,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 96,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 97,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 98,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 99,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 100,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 101,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 102,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 103,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 104,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 105,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 106,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 107,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 108,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 109,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 110,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 111,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 112,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 113,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 114,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 115,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 116,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 117,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 118,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 119,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 120,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 121,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 122,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 123,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 124,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 125,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 126,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 127,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 128,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 129,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 130,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 131,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 132,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 133,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 134,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 135,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 136,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 137,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 138,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 139,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 140,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 141,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 142,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 143,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 144,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 145,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 146,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 147,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 148,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 149,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 150,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 151,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 152,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 153,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 154,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 155,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 156,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 157,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 158,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 159,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 160,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 161,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 162,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 163,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 164,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 165,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 166,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 167,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 168,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 169,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 170,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 171,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 172,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 173,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 174,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 175,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 176,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 177,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 178,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 179,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 180,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 181,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 182,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 183,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 184,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 185,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 186,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 187,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 188,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 189,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 190,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 191,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 192,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 193,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 194,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 195,
                column: "ExerciseType",
                value: "BodyWeight");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 196,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 197,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 198,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 199,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 200,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 201,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 202,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 203,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 204,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 205,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 206,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 207,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 208,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 209,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 210,
                column: "ExerciseType",
                value: "WeightLifting");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 211,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 212,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 213,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 214,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 215,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 216,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 217,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 218,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 219,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 220,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 221,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 222,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 223,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 224,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 225,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 226,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 227,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 228,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 229,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 230,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 231,
                column: "ExerciseType",
                value: "Cardio");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 232,
                column: "ExerciseType",
                value: "Stretching");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 233,
                column: "ExerciseType",
                value: "Stretching");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 234,
                column: "ExerciseType",
                value: "Stretching");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 235,
                column: "ExerciseType",
                value: "Stretching");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 236,
                column: "ExerciseType",
                value: "Stretching");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 237,
                column: "ExerciseType",
                value: "Stretching");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 238,
                column: "ExerciseType",
                value: "Stretching");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 239,
                column: "ExerciseType",
                value: "Stretching");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 240,
                column: "ExerciseType",
                value: "Stretching");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 241,
                column: "ExerciseType",
                value: "Stretching");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 242,
                column: "ExerciseType",
                value: "Stretching");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 243,
                column: "ExerciseType",
                value: "Stretching");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 244,
                column: "ExerciseType",
                value: "Stretching");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 245,
                column: "ExerciseType",
                value: "Stretching");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 246,
                column: "ExerciseType",
                value: "Stretching");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ExerciseType",
                table: "Exercises",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 2,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 3,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 4,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 5,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 6,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 7,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 8,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 9,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 10,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 11,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 12,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 13,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 14,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 15,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 16,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 17,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 18,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 19,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 20,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 21,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 22,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 23,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 24,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 25,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 26,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 27,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 28,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 29,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 30,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 31,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 32,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 33,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 34,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 35,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 36,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 37,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 38,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 39,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 40,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 41,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 42,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 43,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 44,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 45,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 46,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 47,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 48,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 49,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 50,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 51,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 52,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 53,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 54,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 55,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 56,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 57,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 58,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 59,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 60,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 61,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 62,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 63,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 64,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 65,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 66,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 67,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 68,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 69,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 70,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 71,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 72,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 73,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 74,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 75,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 76,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 77,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 78,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 79,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 80,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 81,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 82,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 83,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 84,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 85,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 86,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 87,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 88,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 89,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 90,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 91,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 92,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 93,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 94,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 95,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 96,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 97,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 98,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 99,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 100,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 101,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 102,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 103,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 104,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 105,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 106,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 107,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 108,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 109,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 110,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 111,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 112,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 113,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 114,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 115,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 116,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 117,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 118,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 119,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 120,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 121,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 122,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 123,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 124,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 125,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 126,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 127,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 128,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 129,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 130,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 131,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 132,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 133,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 134,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 135,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 136,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 137,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 138,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 139,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 140,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 141,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 142,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 143,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 144,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 145,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 146,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 147,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 148,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 149,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 150,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 151,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 152,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 153,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 154,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 155,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 156,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 157,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 158,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 159,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 160,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 161,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 162,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 163,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 164,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 165,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 166,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 167,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 168,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 169,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 170,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 171,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 172,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 173,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 174,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 175,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 176,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 177,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 178,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 179,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 180,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 181,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 182,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 183,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 184,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 185,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 186,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 187,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 188,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 189,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 190,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 191,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 192,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 193,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 194,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 195,
                column: "ExerciseType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 196,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 197,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 198,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 199,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 200,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 201,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 202,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 203,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 204,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 205,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 206,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 207,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 208,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 209,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 210,
                column: "ExerciseType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 211,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 212,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 213,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 214,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 215,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 216,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 217,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 218,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 219,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 220,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 221,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 222,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 223,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 224,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 225,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 226,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 227,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 228,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 229,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 230,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 231,
                column: "ExerciseType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 232,
                column: "ExerciseType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 233,
                column: "ExerciseType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 234,
                column: "ExerciseType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 235,
                column: "ExerciseType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 236,
                column: "ExerciseType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 237,
                column: "ExerciseType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 238,
                column: "ExerciseType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 239,
                column: "ExerciseType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 240,
                column: "ExerciseType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 241,
                column: "ExerciseType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 242,
                column: "ExerciseType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 243,
                column: "ExerciseType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 244,
                column: "ExerciseType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 245,
                column: "ExerciseType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 246,
                column: "ExerciseType",
                value: 4);
        }
    }
}
