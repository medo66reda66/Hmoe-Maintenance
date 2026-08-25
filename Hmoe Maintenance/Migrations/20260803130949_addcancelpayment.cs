using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hmoe_Maintenance.Migrations
{
    /// <inheritdoc />
    public partial class addcancelpayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_AspNetUsers_CustomerId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Companies_CompanyId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_MaintenanceRequests_MaintenanceRequestId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_TechnicianProfiles_TechnicianProfileId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameIndex(
                name: "IX_Review_TechnicianProfileId",
                table: "Reviews",
                newName: "IX_Reviews_TechnicianProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_MaintenanceRequestId_CustomerId",
                table: "Reviews",
                newName: "IX_Reviews_MaintenanceRequestId_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_CustomerId",
                table: "Reviews",
                newName: "IX_Reviews_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_CompanyId",
                table: "Reviews",
                newName: "IX_Reviews_CompanyId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledAt",
                table: "Payment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripePaymentIntentId",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeSessionId",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sessionId",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_CustomerId",
                table: "Reviews",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Companies_CompanyId",
                table: "Reviews",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_MaintenanceRequests_MaintenanceRequestId",
                table: "Reviews",
                column: "MaintenanceRequestId",
                principalTable: "MaintenanceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_TechnicianProfiles_TechnicianProfileId",
                table: "Reviews",
                column: "TechnicianProfileId",
                principalTable: "TechnicianProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_CustomerId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Companies_CompanyId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_MaintenanceRequests_MaintenanceRequestId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_TechnicianProfiles_TechnicianProfileId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "CancelledAt",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "StripePaymentIntentId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "StripeSessionId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "sessionId",
                table: "Payment");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_TechnicianProfileId",
                table: "Review",
                newName: "IX_Review_TechnicianProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_MaintenanceRequestId_CustomerId",
                table: "Review",
                newName: "IX_Review_MaintenanceRequestId_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_CustomerId",
                table: "Review",
                newName: "IX_Review_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_CompanyId",
                table: "Review",
                newName: "IX_Review_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_AspNetUsers_CustomerId",
                table: "Review",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Companies_CompanyId",
                table: "Review",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_MaintenanceRequests_MaintenanceRequestId",
                table: "Review",
                column: "MaintenanceRequestId",
                principalTable: "MaintenanceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_TechnicianProfiles_TechnicianProfileId",
                table: "Review",
                column: "TechnicianProfileId",
                principalTable: "TechnicianProfiles",
                principalColumn: "Id");
        }
    }
}
