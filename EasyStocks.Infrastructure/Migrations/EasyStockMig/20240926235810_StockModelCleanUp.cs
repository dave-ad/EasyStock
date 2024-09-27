using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyStocks.Infrastructure.Migrations.EasyStockMig
{
    /// <inheritdoc />
    public partial class StockModelCleanUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockWatchList_Stocks_StockId",
                schema: "ThriftSchema",
                table: "StockWatchList");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Stocks_StockId",
                schema: "ThriftSchema",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "Stocks",
                schema: "ThriftSchema");

            migrationBuilder.CreateTable(
                name: "Stock",
                schema: "ThriftSchema",
                columns: table => new
                {
                    StockId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TickerSymbol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Exchange = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OpeningPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClosingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DayHigh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DayLow = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YearHigh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YearLow = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OutstandingShares = table.Column<int>(type: "int", nullable: false),
                    DividendYield = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    EarningsPerShare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Volume = table.Column<int>(type: "int", nullable: false),
                    Beta = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.StockId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_StockWatchList_Stock_StockId",
                schema: "ThriftSchema",
                table: "StockWatchList",
                column: "StockId",
                principalSchema: "ThriftSchema",
                principalTable: "Stock",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Stock_StockId",
                schema: "ThriftSchema",
                table: "Transaction",
                column: "StockId",
                principalSchema: "ThriftSchema",
                principalTable: "Stock",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockWatchList_Stock_StockId",
                schema: "ThriftSchema",
                table: "StockWatchList");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Stock_StockId",
                schema: "ThriftSchema",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "Stock",
                schema: "ThriftSchema");

            migrationBuilder.CreateTable(
                name: "Stocks",
                schema: "ThriftSchema",
                columns: table => new
                {
                    StockId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClosingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateListed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InitialDeposit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ListedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinimumPurchase = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PricePerUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalUnits = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.StockId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_StockWatchList_Stocks_StockId",
                schema: "ThriftSchema",
                table: "StockWatchList",
                column: "StockId",
                principalSchema: "ThriftSchema",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Stocks_StockId",
                schema: "ThriftSchema",
                table: "Transaction",
                column: "StockId",
                principalSchema: "ThriftSchema",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
