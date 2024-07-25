using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyStocks.Infrastructure.Migrations.EasyStockMig
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "ThriftSchema",
                table: "Stocks",
                newName: "StockId");

            migrationBuilder.CreateTable(
                name: "StockWatchList",
                schema: "ThriftSchema",
                columns: table => new
                {
                    WatchlistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StockId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockWatchList", x => x.WatchlistId);
                    table.ForeignKey(
                        name: "FK_StockWatchList_Stocks_StockId",
                        column: x => x.StockId,
                        principalSchema: "ThriftSchema",
                        principalTable: "Stocks",
                        principalColumn: "StockId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockWatchList_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "ThriftSchema",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockWatchList_StockId",
                schema: "ThriftSchema",
                table: "StockWatchList",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockWatchList_UserId",
                schema: "ThriftSchema",
                table: "StockWatchList",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockWatchList",
                schema: "ThriftSchema");

            migrationBuilder.RenameColumn(
                name: "StockId",
                schema: "ThriftSchema",
                table: "Stocks",
                newName: "Id");
        }
    }
}
