using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hmoe_Maintenance.Migrations
{
    /// <inheritdoc />
    public partial class AddLicenseImageUrlincompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommercialRegistrationImageUrl",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommercialRegistrationNumber",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompletedRequestsCount",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenseImageUrl",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TechnicianCount",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CommercialRegistrationImageUrl",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CommercialRegistrationNumber",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CompletedRequestsCount",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "LicenseImageUrl",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "TechnicianCount",
                table: "Companies");
        }
    }
}
