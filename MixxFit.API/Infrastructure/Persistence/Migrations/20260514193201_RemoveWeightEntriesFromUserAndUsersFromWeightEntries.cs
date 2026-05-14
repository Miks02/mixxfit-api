using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWeightEntriesFromUserAndUsersFromWeightEntries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeightEntries_FitnessProfiles_FitnessProfileId",
                table: "WeightEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_WeightEntries_Users_UserId",
                table: "WeightEntries");

            migrationBuilder.DropIndex(
                name: "IX_WeightEntries_UserId",
                table: "WeightEntries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WeightEntries");

            migrationBuilder.AddForeignKey(
                name: "FK_WeightEntries_FitnessProfiles_FitnessProfileId",
                table: "WeightEntries",
                column: "FitnessProfileId",
                principalTable: "FitnessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeightEntries_FitnessProfiles_FitnessProfileId",
                table: "WeightEntries");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WeightEntries",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_WeightEntries_UserId",
                table: "WeightEntries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeightEntries_FitnessProfiles_FitnessProfileId",
                table: "WeightEntries",
                column: "FitnessProfileId",
                principalTable: "FitnessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WeightEntries_Users_UserId",
                table: "WeightEntries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
