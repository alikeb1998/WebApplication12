using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iz.Online.DataAccess.Migrations
{
    public partial class insono : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "CommisionRate",
                schema: "Symbols",
                table: "Instruments",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<long>(
                name: "Tick",
                schema: "Symbols",
                table: "Instruments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommisionRate",
                schema: "Symbols",
                table: "Instruments");

            migrationBuilder.DropColumn(
                name: "Tick",
                schema: "Symbols",
                table: "Instruments");
        }
    }
}
