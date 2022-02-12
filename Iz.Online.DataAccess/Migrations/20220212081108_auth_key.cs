using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iz.Online.DataAccess.Migrations
{
    public partial class auth_key : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Token",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Token",
                table: "Token",
                column: "Token");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Token",
                table: "Token");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Token",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
