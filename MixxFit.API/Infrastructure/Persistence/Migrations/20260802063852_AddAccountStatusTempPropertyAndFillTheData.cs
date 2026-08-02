using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountStatusTempPropertyAndFillTheData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountStatusTemp",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                UPDATE ""Users""
                SET ""AccountStatusTemp"" = CASE 
                    WHEN ""AccountStatus"" = 1 THEN 'Active'
                    WHEN ""AccountStatus"" = 2 THEN 'Suspended'
                    WHEN ""AccountStatus"" = 3 THEN 'Banned'
                    ELSE 'Unknown'
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountStatusTemp",
                table: "Users");
        }
    }
}
