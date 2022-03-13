using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iz.Online.DataAccess.Migrations
{
    public partial class removeCustomers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerHubs_Customer_CustomerId",
                table: "CustomerHubs");

            migrationBuilder.DropForeignKey(
                name: "FK_InstrumentComments_Customer_CustomerId",
                table: "InstrumentComments");

            migrationBuilder.DropForeignKey(
                name: "FK_WathLists_Customer_CustomerId",
                table: "WathLists");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_WathLists_CustomerId",
                table: "WathLists");

            migrationBuilder.DropIndex(
                name: "IX_InstrumentComments_CustomerId",
                table: "InstrumentComments");

            migrationBuilder.DropIndex(
                name: "IX_CustomerHubs_CustomerId",
                table: "CustomerHubs");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "WathLists",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "InstrumentComments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "CustomerHubs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "WathLists",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "InstrumentComments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "CustomerHubs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LocalToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OmsId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OmsToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenExpireDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WathLists_CustomerId",
                table: "WathLists",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentComments_CustomerId",
                table: "InstrumentComments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerHubs_CustomerId",
                table: "CustomerHubs",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerHubs_Customer_CustomerId",
                table: "CustomerHubs",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstrumentComments_Customer_CustomerId",
                table: "InstrumentComments",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WathLists_Customer_CustomerId",
                table: "WathLists",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
