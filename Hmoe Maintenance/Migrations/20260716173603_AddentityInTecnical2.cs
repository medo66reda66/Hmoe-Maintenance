using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hmoe_Maintenance.Migrations
{
    /// <inheritdoc />
    public partial class AddentityInTecnical2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RevenueShare",
                table: "TechnicianProfiles",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RevenueShare",
                table: "TechnicianProfiles");
        }
    }
}
