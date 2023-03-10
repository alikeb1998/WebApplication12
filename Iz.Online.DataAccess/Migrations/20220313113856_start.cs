using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iz.Online.DataAccess.Migrations
{
    public partial class start : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Symbols");

            migrationBuilder.CreateTable(
                name: "AppConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerHubs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HubId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerHubs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exceptions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExceptionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exceptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentBourse",
                schema: "Symbols",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BourseId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    borse = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentBourse", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentSectors",
                schema: "Symbols",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectorId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentSectors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentSubSectors",
                schema: "Symbols",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubSectorId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentSubSectors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstrumentId = table.Column<long>(type: "bigint", nullable: false),
                    OrderSide = table.Column<int>(type: "int", nullable: false),
                    OrderType = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    ValidityType = table.Column<int>(type: "int", nullable: false),
                    ValidityDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisclosedQuantity = table.Column<int>(type: "int", nullable: false),
                    RegisterOrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    Isr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateOrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OmsResponseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OmsQty = table.Column<long>(type: "bigint", nullable: false),
                    OmsPrice = table.Column<long>(type: "bigint", nullable: false),
                    StatusCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Token",
                columns: table => new
                {
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.Token);
                });

            migrationBuilder.CreateTable(
                name: "WathLists",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WatchListName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WathLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instruments",
                schema: "Symbols",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstrumentId = table.Column<long>(type: "bigint", nullable: false),
                    SymbolName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Isin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BaseVolume = table.Column<long>(type: "bigint", nullable: false),
                    SubSectorId = table.Column<int>(type: "int", nullable: true),
                    SectorId = table.Column<int>(type: "int", nullable: true),
                    BourseId = table.Column<int>(type: "int", nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuyCommisionRate = table.Column<float>(type: "real", nullable: false),
                    SellCommisionRate = table.Column<float>(type: "real", nullable: false),
                    Tick = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instruments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instruments_InstrumentBourse_BourseId",
                        column: x => x.BourseId,
                        principalSchema: "Symbols",
                        principalTable: "InstrumentBourse",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Instruments_InstrumentSectors_SectorId",
                        column: x => x.SectorId,
                        principalSchema: "Symbols",
                        principalTable: "InstrumentSectors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Instruments_InstrumentSubSectors_SubSectorId",
                        column: x => x.SubSectorId,
                        principalSchema: "Symbols",
                        principalTable: "InstrumentSubSectors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InstrumentComments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommentText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstrumentId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstrumentComments_Instruments_InstrumentId",
                        column: x => x.InstrumentId,
                        principalSchema: "Symbols",
                        principalTable: "Instruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WatchListsInstruments",
                columns: table => new
                {
                    InstrumentId = table.Column<long>(type: "bigint", nullable: false),
                    WatchListId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchListsInstruments", x => new { x.InstrumentId, x.WatchListId });
                    table.ForeignKey(
                        name: "FK_WatchListsInstruments_Instruments_InstrumentId",
                        column: x => x.InstrumentId,
                        principalSchema: "Symbols",
                        principalTable: "Instruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchListsInstruments_WathLists_WatchListId",
                        column: x => x.WatchListId,
                        principalTable: "WathLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentComments_InstrumentId",
                table: "InstrumentComments",
                column: "InstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_BourseId",
                schema: "Symbols",
                table: "Instruments",
                column: "BourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_SectorId",
                schema: "Symbols",
                table: "Instruments",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_SubSectorId",
                schema: "Symbols",
                table: "Instruments",
                column: "SubSectorId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchListsInstruments_WatchListId",
                table: "WatchListsInstruments",
                column: "WatchListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppConfigs");

            migrationBuilder.DropTable(
                name: "CustomerHubs");

            migrationBuilder.DropTable(
                name: "Exceptions");

            migrationBuilder.DropTable(
                name: "InstrumentComments");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Token");

            migrationBuilder.DropTable(
                name: "WatchListsInstruments");

            migrationBuilder.DropTable(
                name: "Instruments",
                schema: "Symbols");

            migrationBuilder.DropTable(
                name: "WathLists");

            migrationBuilder.DropTable(
                name: "InstrumentBourse",
                schema: "Symbols");

            migrationBuilder.DropTable(
                name: "InstrumentSectors",
                schema: "Symbols");

            migrationBuilder.DropTable(
                name: "InstrumentSubSectors",
                schema: "Symbols");
        }
    }
}
