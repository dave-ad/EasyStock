using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyStocks.Infrastructure.Migrations.EasyStockMig
{
    /// <inheritdoc />
    public partial class UpdateBrokerRoleTypeandGenderEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrokerType",
                schema: "ThriftSchema",
                table: "Broker");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                schema: "ThriftSchema",
                table: "User",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                schema: "ThriftSchema",
                table: "User",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BrokerType",
                schema: "ThriftSchema",
                table: "Broker",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
