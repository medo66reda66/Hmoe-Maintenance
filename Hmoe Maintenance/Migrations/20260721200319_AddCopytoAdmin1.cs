using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hmoe_Maintenance.Migrations
{
    /// <inheritdoc />
    public partial class AddCopytoAdmin1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TechnicianProfileCopyId",
                table: "TechnicianServices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "companyCopies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AverageRating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalReviews = table.Column<int>(type: "int", nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LicenseImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommercialRegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommercialRegistrationImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TechnicianCount = table.Column<int>(type: "int", nullable: false),
                    CompletedRequestsCount = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companyCopies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_companyCopies_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TechnicianProfileCopies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalIdFrontImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalIdBackImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TechnicianDocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ApprovedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevenueShare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumper = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AverageRating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCompletedJobs = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicianProfileCopies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicianProfileCopies_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TechnicianProfileCopies_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianServices_TechnicianProfileCopyId",
                table: "TechnicianServices",
                column: "TechnicianProfileCopyId");

            migrationBuilder.CreateIndex(
                name: "IX_companyCopies_ApplicationUserId",
                table: "companyCopies",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianProfileCopies_CompanyId",
                table: "TechnicianProfileCopies",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianProfileCopies_UserId",
                table: "TechnicianProfileCopies",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianServices_TechnicianProfileCopies_TechnicianProfileCopyId",
                table: "TechnicianServices",
                column: "TechnicianProfileCopyId",
                principalTable: "TechnicianProfileCopies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianServices_TechnicianProfileCopies_TechnicianProfileCopyId",
                table: "TechnicianServices");

            migrationBuilder.DropTable(
                name: "companyCopies");

            migrationBuilder.DropTable(
                name: "TechnicianProfileCopies");

            migrationBuilder.DropIndex(
                name: "IX_TechnicianServices_TechnicianProfileCopyId",
                table: "TechnicianServices");

            migrationBuilder.DropColumn(
                name: "TechnicianProfileCopyId",
                table: "TechnicianServices");
        }
    }
}
