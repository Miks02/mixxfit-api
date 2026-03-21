using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeSearchingCaseInsensitive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:CollationDefinition:my_case_insensitive", "en-u-ks-level2,en-u-ks-level2,icu,False");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Exercises",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                collation: "my_case_insensitive",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:CollationDefinition:my_case_insensitive", "en-u-ks-level2,en-u-ks-level2,icu,False");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Exercises",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldCollation: "my_case_insensitive");
        }
    }
}
