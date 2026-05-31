using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeFitnessProfileIdNullableInWeightEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeightEntries_FitnessProfiles_FitnessProfileId",
                table: "WeightEntries");

            migrationBuilder.AlterColumn<int>(
                name: "FitnessProfileId",
                table: "WeightEntries",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_WeightEntries_FitnessProfiles_FitnessProfileId",
                table: "WeightEntries",
                column: "FitnessProfileId",
                principalTable: "FitnessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeightEntries_FitnessProfiles_FitnessProfileId",
                table: "WeightEntries");

            migrationBuilder.AlterColumn<int>(
                name: "FitnessProfileId",
                table: "WeightEntries",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WeightEntries_FitnessProfiles_FitnessProfileId",
                table: "WeightEntries",
                column: "FitnessProfileId",
                principalTable: "FitnessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
