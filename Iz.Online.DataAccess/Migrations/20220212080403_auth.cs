using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iz.Online.DataAccess.Migrations
{
    public partial class auth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "InstrumentId",
                table: "Orders",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Token",
                columns: table => new
                {
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Token");

            migrationBuilder.AlterColumn<int>(
                name: "InstrumentId",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
