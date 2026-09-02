using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hmoe_Maintenance.Migrations
{
    /// <inheritdoc />
    public partial class CompanyCopyIdintech : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyCopyId",
                table: "TechnicianProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianProfiles_CompanyCopyId",
                table: "TechnicianProfiles",
                column: "CompanyCopyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianProfiles_companyCopies_CompanyCopyId",
                table: "TechnicianProfiles",
                column: "CompanyCopyId",
                principalTable: "companyCopies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianProfiles_companyCopies_CompanyCopyId",
                table: "TechnicianProfiles");

            migrationBuilder.DropIndex(
                name: "IX_TechnicianProfiles_CompanyCopyId",
                table: "TechnicianProfiles");

            migrationBuilder.DropColumn(
                name: "CompanyCopyId",
                table: "TechnicianProfiles");
        }
    }
}
