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
                name: "AspNetRoles",
                schema: "ThriftSchema",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

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
                name: "AspNetRoleClaims",
                schema: "ThriftSchema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "ThriftSchema",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "ThriftSchema",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name_Last = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name_First = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name_Others = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    MobileNumber_Value = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    MobileNumber_Hash = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    PositionInOrg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfEmployment = table.Column<DateOnly>(type: "date", nullable: true),
                    BrokerId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Broker_BrokerId",
                        column: x => x.BrokerId,
                        principalSchema: "ThriftSchema",
                        principalTable: "Broker",
                        principalColumn: "BrokerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "ThriftSchema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "ThriftSchema",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "ThriftSchema",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "ThriftSchema",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "ThriftSchema",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "ThriftSchema",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "ThriftSchema",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "ThriftSchema",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "ThriftSchema",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "ThriftSchema",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "ThriftSchema",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "ThriftSchema",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "ThriftSchema",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "ThriftSchema",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "ThriftSchema",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_User_BrokerId",
                schema: "ThriftSchema",
                table: "User",
                column: "BrokerId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "ThriftSchema",
                table: "User",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "ThriftSchema");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "ThriftSchema");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "ThriftSchema");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "ThriftSchema");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "ThriftSchema");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "ThriftSchema");

            migrationBuilder.DropTable(
                name: "User",
                schema: "ThriftSchema");

            migrationBuilder.DropTable(
                name: "Broker",
                schema: "ThriftSchema");
        }
    }
}
