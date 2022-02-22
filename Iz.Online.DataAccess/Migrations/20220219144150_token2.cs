using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iz.Online.DataAccess.Migrations
{
    public partial class token2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Token",
                table: "Customer",
                newName: "LocalToken");

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenExpireDate",
                table: "Customer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenExpireDate",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "LocalToken",
                table: "Customer",
                newName: "Token");
        }
    }
}
