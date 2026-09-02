using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hmoe_Maintenance.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequests_Companies_CompanyId",
                table: "MaintenanceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Companies_CompanyId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_TechnicianProfiles_TechnicianProfileId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianServices_TechnicianProfileCopies_TechnicianProfileCopyId",
                table: "TechnicianServices");

            migrationBuilder.DropIndex(
                name: "IX_TechnicianServices_TechnicianProfileCopyId",
                table: "TechnicianServices");

            migrationBuilder.DropColumn(
                name: "TechnicianProfileCopyId",
                table: "TechnicianServices");

            migrationBuilder.RenameColumn(
                name: "TechnicianProfileId",
                table: "Reviews",
                newName: "TechnicianProfileCopyId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Reviews",
                newName: "CompanyCopyId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_TechnicianProfileId",
                table: "Reviews",
                newName: "IX_Reviews_TechnicianProfileCopyId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_CompanyId",
                table: "Reviews",
                newName: "IX_Reviews_CompanyCopyId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "MaintenanceRequests",
                newName: "CompanycopyId");

            migrationBuilder.RenameIndex(
                name: "IX_MaintenanceRequests_CompanyId",
                table: "MaintenanceRequests",
                newName: "IX_MaintenanceRequests_CompanycopyId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequests_companyCopies_CompanycopyId",
                table: "MaintenanceRequests",
                column: "CompanycopyId",
                principalTable: "companyCopies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_TechnicianProfileCopies_TechnicianProfileCopyId",
                table: "Reviews",
                column: "TechnicianProfileCopyId",
                principalTable: "TechnicianProfileCopies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_companyCopies_CompanyCopyId",
                table: "Reviews",
                column: "CompanyCopyId",
                principalTable: "companyCopies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequests_companyCopies_CompanycopyId",
                table: "MaintenanceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_TechnicianProfileCopies_TechnicianProfileCopyId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_companyCopies_CompanyCopyId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "TechnicianProfileCopyId",
                table: "Reviews",
                newName: "TechnicianProfileId");

            migrationBuilder.RenameColumn(
                name: "CompanyCopyId",
                table: "Reviews",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_TechnicianProfileCopyId",
                table: "Reviews",
                newName: "IX_Reviews_TechnicianProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_CompanyCopyId",
                table: "Reviews",
                newName: "IX_Reviews_CompanyId");

            migrationBuilder.RenameColumn(
                name: "CompanycopyId",
                table: "MaintenanceRequests",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_MaintenanceRequests_CompanycopyId",
                table: "MaintenanceRequests",
                newName: "IX_MaintenanceRequests_CompanyId");

            migrationBuilder.AddColumn<int>(
                name: "TechnicianProfileCopyId",
                table: "TechnicianServices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianServices_TechnicianProfileCopyId",
                table: "TechnicianServices",
                column: "TechnicianProfileCopyId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequests_Companies_CompanyId",
                table: "MaintenanceRequests",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Companies_CompanyId",
                table: "Reviews",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_TechnicianProfiles_TechnicianProfileId",
                table: "Reviews",
                column: "TechnicianProfileId",
                principalTable: "TechnicianProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianServices_TechnicianProfileCopies_TechnicianProfileCopyId",
                table: "TechnicianServices",
                column: "TechnicianProfileCopyId",
                principalTable: "TechnicianProfileCopies",
                principalColumn: "Id");
        }
    }
}
