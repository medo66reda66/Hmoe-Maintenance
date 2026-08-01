using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hmoe_Maintenance.Migrations
{
    /// <inheritdoc />
    public partial class AddentityInTecnical : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TechnicianServices_TechnicianProfileId",
                table: "TechnicianServices");

            migrationBuilder.AddColumn<string>(
                name: "ApprovedByUserId",
                table: "TechnicianProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalIdBackImageUrl",
                table: "TechnicianProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NationalIdFrontImageUrl",
                table: "TechnicianProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "TechnicianProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "TechnicianProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearsOfExperience",
                table: "TechnicianProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianServices_TechnicianProfileId",
                table: "TechnicianServices",
                column: "TechnicianProfileId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TechnicianServices_TechnicianProfileId",
                table: "TechnicianServices");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserId",
                table: "TechnicianProfiles");

            migrationBuilder.DropColumn(
                name: "NationalIdBackImageUrl",
                table: "TechnicianProfiles");

            migrationBuilder.DropColumn(
                name: "NationalIdFrontImageUrl",
                table: "TechnicianProfiles");

            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "TechnicianProfiles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TechnicianProfiles");

            migrationBuilder.DropColumn(
                name: "YearsOfExperience",
                table: "TechnicianProfiles");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianServices_TechnicianProfileId",
                table: "TechnicianServices",
                column: "TechnicianProfileId");
        }
    }
}
