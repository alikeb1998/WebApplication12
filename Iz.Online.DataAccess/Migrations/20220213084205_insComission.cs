using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iz.Online.DataAccess.Migrations
{
    public partial class insComission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommisionRate",
                schema: "Symbols",
                table: "Instruments",
                newName: "SellCommisionRate");

            migrationBuilder.AddColumn<float>(
                name: "BuyCommisionRate",
                schema: "Symbols",
                table: "Instruments",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyCommisionRate",
                schema: "Symbols",
                table: "Instruments");

            migrationBuilder.RenameColumn(
                name: "SellCommisionRate",
                schema: "Symbols",
                table: "Instruments",
                newName: "CommisionRate");
        }
    }
}
