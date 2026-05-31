using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MoveSpecificUserPropertiesToFitnessProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FitnessProfileId",
                table: "Workouts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FitnessProfileId",
                table: "WeightEntries",
                type: "integer",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "FitnessProfileId",
                table: "Exercises",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 2,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 3,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 4,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 5,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 6,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 7,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 8,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 9,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 10,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 11,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 12,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 13,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 14,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 15,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 16,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 17,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 18,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 19,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 20,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 21,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 22,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 23,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 24,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 25,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 26,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 27,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 28,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 29,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 30,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 31,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 32,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 33,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 34,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 35,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 36,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 37,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 38,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 39,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 40,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 41,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 42,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 43,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 44,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 45,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 46,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 47,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 48,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 49,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 50,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 51,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 52,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 53,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 54,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 55,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 56,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 57,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 58,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 59,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 60,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 61,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 62,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 63,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 64,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 65,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 66,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 67,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 68,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 69,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 70,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 71,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 72,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 73,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 74,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 75,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 76,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 77,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 78,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 79,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 80,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 81,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 82,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 83,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 84,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 85,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 86,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 87,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 88,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 89,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 90,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 91,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 92,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 93,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 94,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 95,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 96,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 97,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 98,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 99,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 100,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 101,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 102,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 103,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 104,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 105,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 106,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 107,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 108,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 109,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 110,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 111,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 112,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 113,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 114,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 115,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 116,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 117,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 118,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 119,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 120,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 121,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 122,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 123,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 124,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 125,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 126,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 127,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 128,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 129,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 130,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 131,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 132,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 133,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 134,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 135,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 136,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 137,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 138,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 139,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 140,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 141,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 142,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 143,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 144,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 145,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 146,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 147,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 148,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 149,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 150,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 151,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 152,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 153,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 154,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 155,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 156,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 157,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 158,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 159,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 160,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 161,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 162,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 163,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 164,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 165,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 166,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 167,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 168,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 169,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 170,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 171,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 172,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 173,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 174,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 175,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 176,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 177,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 178,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 179,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 180,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 181,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 182,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 183,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 184,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 185,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 186,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 187,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 188,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 189,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 190,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 191,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 192,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 193,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 194,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 195,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 196,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 197,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 198,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 199,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 200,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 201,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 202,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 203,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 204,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 205,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 206,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 207,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 208,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 209,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 210,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 211,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 212,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 213,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 214,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 215,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 216,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 217,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 218,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 219,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 220,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 221,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 222,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 223,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 224,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 225,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 226,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 227,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 228,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 229,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 230,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 231,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 232,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 233,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 234,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 235,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 236,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 237,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 238,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 239,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 240,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 241,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 242,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 243,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 244,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 245,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 246,
                column: "FitnessProfileId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_FitnessProfileId",
                table: "Workouts",
                column: "FitnessProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightEntries_FitnessProfileId",
                table: "WeightEntries",
                column: "FitnessProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_FitnessProfileId",
                table: "Exercises",
                column: "FitnessProfileId");

            migrationBuilder.Sql(@"
                UPDATE ""WeightEntries"" we
                SET ""FitnessProfileId"" = fp.""Id""
                FROM ""FitnessProfiles"" fp
                WHERE fp.""UserId"" = we.""UserId"";
            ");

            migrationBuilder.Sql(@"
                UPDATE ""Workouts"" w
                SET ""FitnessProfileId"" = fp.""Id""
                FROM ""FitnessProfiles"" fp
                WHERE fp.""UserId"" = w.""UserId"";
            ");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_FitnessProfiles_FitnessProfileId",
                table: "Exercises",
                column: "FitnessProfileId",
                principalTable: "FitnessProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WeightEntries_FitnessProfiles_FitnessProfileId",
                table: "WeightEntries",
                column: "FitnessProfileId",
                principalTable: "FitnessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_FitnessProfiles_FitnessProfileId",
                table: "Workouts",
                column: "FitnessProfileId",
                principalTable: "FitnessProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_FitnessProfiles_FitnessProfileId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_WeightEntries_FitnessProfiles_FitnessProfileId",
                table: "WeightEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_FitnessProfiles_FitnessProfileId",
                table: "Workouts");

            migrationBuilder.DropIndex(
                name: "IX_Workouts_FitnessProfileId",
                table: "Workouts");

            migrationBuilder.DropIndex(
                name: "IX_WeightEntries_FitnessProfileId",
                table: "WeightEntries");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_FitnessProfileId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "FitnessProfileId",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "FitnessProfileId",
                table: "WeightEntries");

            migrationBuilder.DropColumn(
                name: "FitnessProfileId",
                table: "Exercises");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
