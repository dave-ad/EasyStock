using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyStocks.Infrastructure.Migrations.EasyStockMig
{
    /// <inheritdoc />
    public partial class Initial : Migration
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
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEmail_Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompanyEmail_Hash = table.Column<int>(type: "int", nullable: true),
                    CompanyMobileNumber_Value = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    CompanyMobileNumber_Hash = table.Column<int>(type: "int", nullable: true),
                    Company_Street_Number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Company_Street_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Company_City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Company_State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Company_ZipCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    CACRegistrationNumber_Value = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    CACRegistrationNumber_Hash = table.Column<int>(type: "int", nullable: true),
                    StockBrokerLicense_Value = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    StockBrokerLicense_Hash = table.Column<int>(type: "int", nullable: true),
                    DateCertified = table.Column<DateOnly>(type: "date", nullable: true),
                    Business_Street_Number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Business_Street_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Business_City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Business_State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Business_ZipCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "User",
                schema: "ThriftSchema",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Last = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name_First = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name_Others = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email_Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email_Hash = table.Column<int>(type: "int", nullable: false),
                    MobileNumber_Value = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    MobileNumber_Hash = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    PositionInOrg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfEmployment = table.Column<DateOnly>(type: "date", nullable: true),
                    BrokerId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_Broker_BrokerId",
                        column: x => x.BrokerId,
                        principalSchema: "ThriftSchema",
                        principalTable: "Broker",
                        principalColumn: "BrokerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_BrokerId",
                schema: "ThriftSchema",
                table: "User",
                column: "BrokerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User",
                schema: "ThriftSchema");

            migrationBuilder.DropTable(
                name: "Broker",
                schema: "ThriftSchema");
        }
    }
}
