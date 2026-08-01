using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hmoe_Maintenance.Migrations
{
    /// <inheritdoc />
    public partial class AddCoulumnEmailPhomINtec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "TechnicianProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Fullname",
                table: "TechnicianProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumper",
                table: "TechnicianProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "TechnicianProfiles");

            migrationBuilder.DropColumn(
                name: "Fullname",
                table: "TechnicianProfiles");

            migrationBuilder.DropColumn(
                name: "PhoneNumper",
                table: "TechnicianProfiles");
        }
    }
}
