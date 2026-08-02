using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConvertIntEnumGenderToStringInFitnessProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GenderTemp",
                table: "FitnessProfiles",
                type: "text",
                nullable: true
            );

            migrationBuilder.Sql(@"
                UPDATE ""FitnessProfiles""
                SET ""GenderTemp"" = CASE 
                    WHEN ""Gender"" = 1 THEN 'Male'
                    WHEN ""Gender"" = 2 THEN 'Female'
                    ELSE 'Other'
                END
            ");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "FitnessProfiles",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.Sql(@"
                UPDATE ""FitnessProfiles""
                SET ""Gender"" = ""GenderTemp""
            ");
            
            migrationBuilder.DropColumn(
                name: "GenderTemp",
                table: "FitnessProfiles");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "FitnessProfiles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
