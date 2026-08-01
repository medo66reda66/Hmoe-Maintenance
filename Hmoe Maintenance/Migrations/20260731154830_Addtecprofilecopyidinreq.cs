using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hmoe_Maintenance.Migrations
{
    /// <inheritdoc />
    public partial class Addtecprofilecopyidinreq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "technicianProfileCopyId",
                table: "MaintenanceRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_technicianProfileCopyId",
                table: "MaintenanceRequests",
                column: "technicianProfileCopyId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequests_TechnicianProfileCopies_technicianProfileCopyId",
                table: "MaintenanceRequests",
                column: "technicianProfileCopyId",
                principalTable: "TechnicianProfileCopies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequests_TechnicianProfileCopies_technicianProfileCopyId",
                table: "MaintenanceRequests");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceRequests_technicianProfileCopyId",
                table: "MaintenanceRequests");

            migrationBuilder.DropColumn(
                name: "technicianProfileCopyId",
                table: "MaintenanceRequests");
        }
    }
}
