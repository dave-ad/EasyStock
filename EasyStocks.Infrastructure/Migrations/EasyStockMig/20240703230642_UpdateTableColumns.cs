using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyStocks.Infrastructure.Migrations.EasyStockMig
{
    /// <inheritdoc />
    public partial class UpdateTableColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Street_Number",
                schema: "ThriftSchema",
                table: "Broker",
                newName: "Company_Street_Number");

            migrationBuilder.RenameColumn(
                name: "Street_Name",
                schema: "ThriftSchema",
                table: "Broker",
                newName: "Company_Street_Name");

            migrationBuilder.RenameColumn(
                name: "CompanyAddress_ZipCode",
                schema: "ThriftSchema",
                table: "Broker",
                newName: "Company_ZipCode");

            migrationBuilder.RenameColumn(
                name: "CompanyAddress_State",
                schema: "ThriftSchema",
                table: "Broker",
                newName: "Company_State");

            migrationBuilder.RenameColumn(
                name: "CompanyAddress_City",
                schema: "ThriftSchema",
                table: "Broker",
                newName: "Company_City");

            migrationBuilder.RenameColumn(
                name: "BusinessAddress_ZipCode",
                schema: "ThriftSchema",
                table: "Broker",
                newName: "Business_ZipCode");

            migrationBuilder.RenameColumn(
                name: "BusinessAddress_State",
                schema: "ThriftSchema",
                table: "Broker",
                newName: "Business_State");

            migrationBuilder.RenameColumn(
                name: "BusinessAddress_City",
                schema: "ThriftSchema",
                table: "Broker",
                newName: "Business_City");

            migrationBuilder.AddColumn<string>(
                name: "Business_Street_Name",
                schema: "ThriftSchema",
                table: "Broker",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Business_Street_Number",
                schema: "ThriftSchema",
                table: "Broker",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Business_Street_Name",
                schema: "ThriftSchema",
                table: "Broker");

            migrationBuilder.DropColumn(
                name: "Business_Street_Number",
                schema: "ThriftSchema",
                table: "Broker");

            migrationBuilder.RenameColumn(
                name: "Company_ZipCode",
                schema: "ThriftSchema",
                table: "Broker",
                newName: "CompanyAddress_ZipCode");

            migrationBuilder.RenameColumn(
                name: "Company_Street_Number",
                schema: "ThriftSchema",
                table: "Broker",
                newName: "Street_Number");

            migrationBuilder.RenameColumn(
                name: "Company_Street_Name",
                schema: "ThriftSchema",
                table: "Broker",
                newName: "Street_Name");

            migrationBuilder.RenameColumn(
                name: "Company_State",
                schema: "ThriftSchema",
                table: "Broker",
                newName: "CompanyAddress_State");

            migrationBuilder.RenameColumn(
                name: "Company_City",
                schema: "ThriftSchema",
                table: "Broker",
                newName: "CompanyAddress_City");

            migrationBuilder.RenameColumn(
                name: "Business_ZipCode",
                schema: "ThriftSchema",
                table: "Broker",
                newName: "BusinessAddress_ZipCode");

            migrationBuilder.RenameColumn(
                name: "Business_State",
                schema: "ThriftSchema",
                table: "Broker",
                newName: "BusinessAddress_State");

            migrationBuilder.RenameColumn(
                name: "Business_City",
                schema: "ThriftSchema",
                table: "Broker",
                newName: "BusinessAddress_City");
        }
    }
}
