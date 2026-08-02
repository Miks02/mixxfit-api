using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOldAccountStatusAndReplaceItWithTempProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountStatus",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.Sql(@"
                UPDATE ""Users""
                SET ""AccountStatus"" = CASE 
                    WHEN ""AccountStatus"" = '1' THEN 'Active'
                    WHEN ""AccountStatus"" = '2' THEN 'Banned'
                    WHEN ""AccountStatus"" = '3' THEN 'Suspended'
                    ELSE 'Active'
                END
            ");
            
            migrationBuilder.DropColumn(
                name: "AccountStatusTemp",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AccountStatus",
                table: "Users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "AccountStatusTemp",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
