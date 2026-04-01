using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixCheckConstraintForDurationSecondsInSetEntries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_SetEntries_DurationSeconds_Positive",
                table: "SetEntries");

            migrationBuilder.AddCheckConstraint(
                name: "CK_SetEntries_DurationSeconds_Positive",
                table: "SetEntries",
                sql: "\"DurationSeconds\" IS NULL OR \"DurationSeconds\" > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_SetEntries_DurationSeconds_Positive",
                table: "SetEntries");

            migrationBuilder.AddCheckConstraint(
                name: "CK_SetEntries_DurationSeconds_Positive",
                table: "SetEntries",
                sql: "\"DurationSeconds\" IS NULL OR \"DurationSeconds\" BETWEEN 0 AND 59");
        }
    }
}
