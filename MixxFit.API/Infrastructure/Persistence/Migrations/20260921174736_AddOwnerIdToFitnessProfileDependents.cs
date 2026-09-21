using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerIdToFitnessProfileDependents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Workouts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "WeightEntries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Exercises",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "WorkoutTemplates",
                type: "text",
                nullable: true);

            // Backfill OwnerId with the owning user's id. Rows without a FitnessProfile (system seed data) stay NULL.
            migrationBuilder.Sql("""
                UPDATE "Workouts" AS t
                SET "OwnerId" = fp."UserId"
                FROM "FitnessProfiles" AS fp
                WHERE t."FitnessProfileId" = fp."Id";
                """);

            migrationBuilder.Sql("""
                UPDATE "WeightEntries" AS t
                SET "OwnerId" = fp."UserId"
                FROM "FitnessProfiles" AS fp
                WHERE t."FitnessProfileId" = fp."Id";
                """);

            migrationBuilder.Sql("""
                UPDATE "Exercises" AS t
                SET "OwnerId" = fp."UserId"
                FROM "FitnessProfiles" AS fp
                WHERE t."FitnessProfileId" = fp."Id";
                """);

            migrationBuilder.Sql("""
                UPDATE "WorkoutTemplates" AS t
                SET "OwnerId" = fp."UserId"
                FROM "FitnessProfiles" AS fp
                WHERE t."FitnessProfileId" = fp."Id";
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "WeightEntries");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "WorkoutTemplates");
        }
    }
}
