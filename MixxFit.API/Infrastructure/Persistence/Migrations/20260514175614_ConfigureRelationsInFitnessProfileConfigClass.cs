using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureRelationsInFitnessProfileConfigClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_FitnessProfiles_FitnessProfileId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_FitnessProfiles_FitnessProfileId",
                table: "Workouts");

            migrationBuilder.AlterColumn<int>(
                name: "FitnessProfileId",
                table: "Workouts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_FitnessProfiles_FitnessProfileId",
                table: "Exercises",
                column: "FitnessProfileId",
                principalTable: "FitnessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_FitnessProfiles_FitnessProfileId",
                table: "Workouts",
                column: "FitnessProfileId",
                principalTable: "FitnessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_FitnessProfiles_FitnessProfileId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_FitnessProfiles_FitnessProfileId",
                table: "Workouts");

            migrationBuilder.AlterColumn<int>(
                name: "FitnessProfileId",
                table: "Workouts",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_FitnessProfiles_FitnessProfileId",
                table: "Exercises",
                column: "FitnessProfileId",
                principalTable: "FitnessProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_FitnessProfiles_FitnessProfileId",
                table: "Workouts",
                column: "FitnessProfileId",
                principalTable: "FitnessProfiles",
                principalColumn: "Id");
        }
    }
}
