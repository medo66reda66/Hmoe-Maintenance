using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hmoe_Maintenance.Migrations
{
    /// <inheritdoc />
    public partial class Addpaymentinreq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PaymentApproved",
                table: "MaintenanceRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PaymentRejected",
                table: "MaintenanceRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentApproved",
                table: "MaintenanceRequests");

            migrationBuilder.DropColumn(
                name: "PaymentRejected",
                table: "MaintenanceRequests");
        }
    }
}
