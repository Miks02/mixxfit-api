using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDateOfBirthToFitnessProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DailyCalorieGoal",
                table: "FitnessProfiles",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "FitnessProfiles",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DailyCalorieGoal",
                table: "FitnessProfiles");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "FitnessProfiles");
        }
    }
}
