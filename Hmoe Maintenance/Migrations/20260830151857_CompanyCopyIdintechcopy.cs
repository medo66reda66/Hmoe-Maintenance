using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hmoe_Maintenance.Migrations
{
    /// <inheritdoc />
    public partial class CompanyCopyIdintechcopy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyCopyId",
                table: "TechnicianProfileCopies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianProfileCopies_CompanyCopyId",
                table: "TechnicianProfileCopies",
                column: "CompanyCopyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianProfileCopies_companyCopies_CompanyCopyId",
                table: "TechnicianProfileCopies",
                column: "CompanyCopyId",
                principalTable: "companyCopies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianProfileCopies_companyCopies_CompanyCopyId",
                table: "TechnicianProfileCopies");

            migrationBuilder.DropIndex(
                name: "IX_TechnicianProfileCopies_CompanyCopyId",
                table: "TechnicianProfileCopies");

            migrationBuilder.DropColumn(
                name: "CompanyCopyId",
                table: "TechnicianProfileCopies");
        }
    }
}
