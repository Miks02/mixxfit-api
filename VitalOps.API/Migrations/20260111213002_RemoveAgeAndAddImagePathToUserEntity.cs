using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VitalOps.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAgeAndAddImagePathToUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Age",
                table: "Users",
                newName: "AccountStatus");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "AccountStatus",
                table: "Users",
                newName: "Age");
        }
    }
}
