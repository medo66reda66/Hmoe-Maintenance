using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hmoe_Maintenance.Migrations
{
    /// <inheritdoc />
    public partial class EntitypublicTechnicianDocumentUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TechnicianDocumentUrl",
                table: "TechnicianProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TechnicianDocumentUrl",
                table: "TechnicianProfiles");
        }
    }
}
