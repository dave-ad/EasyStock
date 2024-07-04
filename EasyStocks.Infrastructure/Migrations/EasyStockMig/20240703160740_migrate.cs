using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyStocks.Infrastructure.Migrations.EasyStockMig
{
    /// <inheritdoc />
    public partial class migrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ThriftSchema");

            migrationBuilder.CreateTable(
                name: "Broker",
                schema: "ThriftSchema",
                columns: table => new
                {
                    BrokerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Last = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name_First = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name_Others = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email_Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email_Hash = table.Column<int>(type: "int", nullable: false),
                    MobileNumber_Value = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    MobileNumber_Hash = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEmail_Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompanyEmail_Hash = table.Column<int>(type: "int", nullable: true),
                    CompanyMobileNumber_Value = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    CompanyMobileNumber_Hash = table.Column<int>(type: "int", nullable: true),
                    Street_Number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Street_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompanyAddress_City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CompanyAddress_State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CompanyAddress_ZipCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    CACRegistrationNumber_Value = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    CACRegistrationNumber_Hash = table.Column<int>(type: "int", nullable: true),
                    StockBrokerLicenseNumber_Value = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    StockBrokerLicenseNumber_Hash = table.Column<int>(type: "int", nullable: true),
                    DateCertified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PositionInOrg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfEmployment = table.Column<DateOnly>(type: "date", nullable: true),
                    BusinessAddress_City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BusinessAddress_State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BusinessAddress_ZipCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    ProfessionalQualification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrokerType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Broker", x => x.BrokerId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Broker",
                schema: "ThriftSchema");
        }
    }
}
