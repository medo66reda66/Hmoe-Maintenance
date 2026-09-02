using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hmoe_Maintenance.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCompanyFromTechnicianProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianProfileCopies_Companies_CompanyId",
                table: "TechnicianProfileCopies");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianProfiles_Companies_CompanyId",
                table: "TechnicianProfiles");

            migrationBuilder.DropIndex(
                name: "IX_TechnicianProfiles_CompanyId",
                table: "TechnicianProfiles");

            migrationBuilder.DropIndex(
                name: "IX_TechnicianProfileCopies_CompanyId",
                table: "TechnicianProfileCopies");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "TechnicianProfiles");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "TechnicianProfileCopies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "TechnicianProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "TechnicianProfileCopies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianProfiles_CompanyId",
                table: "TechnicianProfiles",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianProfileCopies_CompanyId",
                table: "TechnicianProfileCopies",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianProfileCopies_Companies_CompanyId",
                table: "TechnicianProfileCopies",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianProfiles_Companies_CompanyId",
                table: "TechnicianProfiles",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
