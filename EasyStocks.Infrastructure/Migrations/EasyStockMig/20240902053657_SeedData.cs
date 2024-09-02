using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyStocks.Infrastructure.Migrations.EasyStockMig
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions",
                schema: "ThriftSchema");

            migrationBuilder.DropColumn(
                name: "MobileNumber_Hash",
                schema: "ThriftSchema",
                table: "User");

            migrationBuilder.DropColumn(
                name: "MobileNumber_Value",
                schema: "ThriftSchema",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Permissions",
                schema: "ThriftSchema",
                table: "User");

            migrationBuilder.DropColumn(
                name: "SuperAdminLevel",
                schema: "ThriftSchema",
                table: "User");

            migrationBuilder.CreateTable(
                name: "Invoices",
                schema: "ThriftSchema",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PricePerUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceId);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                schema: "ThriftSchema",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PricePerUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPurchase = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransactionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transaction_Stocks_StockId",
                        column: x => x.StockId,
                        principalSchema: "ThriftSchema",
                        principalTable: "Stocks",
                        principalColumn: "StockId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "ThriftSchema",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_StockId",
                schema: "ThriftSchema",
                table: "Transaction",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_UserId",
                schema: "ThriftSchema",
                table: "Transaction",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoices",
                schema: "ThriftSchema");

            migrationBuilder.DropTable(
                name: "Transaction",
                schema: "ThriftSchema");

            migrationBuilder.AddColumn<int>(
                name: "MobileNumber_Hash",
                schema: "ThriftSchema",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MobileNumber_Value",
                schema: "ThriftSchema",
                table: "User",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Permissions",
                schema: "ThriftSchema",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuperAdminLevel",
                schema: "ThriftSchema",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Transactions",
                schema: "ThriftSchema",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<int>(type: "int", nullable: false),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TransactionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_Stocks_StockId",
                        column: x => x.StockId,
                        principalSchema: "ThriftSchema",
                        principalTable: "Stocks",
                        principalColumn: "StockId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_User_Id",
                        column: x => x.Id,
                        principalSchema: "ThriftSchema",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Id",
                schema: "ThriftSchema",
                table: "Transactions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_StockId",
                schema: "ThriftSchema",
                table: "Transactions",
                column: "StockId");
        }
    }
}
