using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hmoe_Maintenance.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationinmaintenance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequestStatusHistory_AspNetUsers_ChangedByUserId",
                table: "MaintenanceRequestStatusHistory");

            migrationBuilder.DropColumn(
                name: "NewStatus",
                table: "MaintenanceRequestStatusHistory");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "MaintenanceRequestStatusHistory");

            migrationBuilder.DropColumn(
                name: "OldStatus",
                table: "MaintenanceRequestStatusHistory");

            migrationBuilder.RenameColumn(
                name: "ChangedByUserId",
                table: "MaintenanceRequestStatusHistory",
                newName: "UploadedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_MaintenanceRequestStatusHistory_ChangedByUserId",
                table: "MaintenanceRequestStatusHistory",
                newName: "IX_MaintenanceRequestStatusHistory_UploadedByUserId");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "MaintenanceRequestStatusHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsAfterWork",
                table: "MaintenanceRequestStatusHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBeforeWork",
                table: "MaintenanceRequestStatusHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BuildingNumber",
                table: "MaintenanceRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "MaintenanceRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Floor",
                table: "MaintenanceRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Governorate",
                table: "MaintenanceRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "MaintenanceRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "MaintenanceRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequestStatusHistory_AspNetUsers_UploadedByUserId",
                table: "MaintenanceRequestStatusHistory",
                column: "UploadedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequestStatusHistory_AspNetUsers_UploadedByUserId",
                table: "MaintenanceRequestStatusHistory");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "MaintenanceRequestStatusHistory");

            migrationBuilder.DropColumn(
                name: "IsAfterWork",
                table: "MaintenanceRequestStatusHistory");

            migrationBuilder.DropColumn(
                name: "IsBeforeWork",
                table: "MaintenanceRequestStatusHistory");

            migrationBuilder.DropColumn(
                name: "BuildingNumber",
                table: "MaintenanceRequests");

            migrationBuilder.DropColumn(
                name: "City",
                table: "MaintenanceRequests");

            migrationBuilder.DropColumn(
                name: "Floor",
                table: "MaintenanceRequests");

            migrationBuilder.DropColumn(
                name: "Governorate",
                table: "MaintenanceRequests");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "MaintenanceRequests");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "MaintenanceRequests");

            migrationBuilder.RenameColumn(
                name: "UploadedByUserId",
                table: "MaintenanceRequestStatusHistory",
                newName: "ChangedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_MaintenanceRequestStatusHistory_UploadedByUserId",
                table: "MaintenanceRequestStatusHistory",
                newName: "IX_MaintenanceRequestStatusHistory_ChangedByUserId");

            migrationBuilder.AddColumn<int>(
                name: "NewStatus",
                table: "MaintenanceRequestStatusHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "MaintenanceRequestStatusHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OldStatus",
                table: "MaintenanceRequestStatusHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequestStatusHistory_AspNetUsers_ChangedByUserId",
                table: "MaintenanceRequestStatusHistory",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
