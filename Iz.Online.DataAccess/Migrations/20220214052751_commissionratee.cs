using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iz.Online.DataAccess.Migrations
{
    public partial class commissionratee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyCommisionRate",
                schema: "Symbols",
                table: "Instruments");

            migrationBuilder.DropColumn(
                name: "SellCommisionRate",
                schema: "Symbols",
                table: "Instruments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "BuyCommisionRate",
                schema: "Symbols",
                table: "Instruments",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SellCommisionRate",
                schema: "Symbols",
                table: "Instruments",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
