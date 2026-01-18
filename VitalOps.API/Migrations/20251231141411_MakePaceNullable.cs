using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VitalOps.API.Migrations
{
    /// <inheritdoc />
    public partial class MakePaceNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PaceMinPerKm",
                table: "ExerciseEntries",
                type: "float(5)",
                precision: 5,
                scale: 2,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float(5)",
                oldPrecision: 5,
                oldScale: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PaceMinPerKm",
                table: "ExerciseEntries",
                type: "float(5)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float(5)",
                oldPrecision: 5,
                oldScale: 2,
                oldNullable: true);
        }
    }
}
