using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.VSA.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangePropertyTypeDailyCalorieGoalFromIntNullableToDoubleNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "DailyCalorieGoal",
                table: "Users",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DailyCalorieGoal",
                table: "Users",
                type: "integer",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);
        }
    }
}
