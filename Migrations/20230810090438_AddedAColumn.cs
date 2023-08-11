using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DOTP_BE.Migrations
{
    public partial class AddedAColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Temp_AttachFile_RecommandDoc1",
                table: "LicenseOnlys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Temp_AttachFile_RecommandDoc2",
                table: "LicenseOnlys",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Temp_AttachFile_RecommandDoc1",
                table: "LicenseOnlys");

            migrationBuilder.DropColumn(
                name: "Temp_AttachFile_RecommandDoc2",
                table: "LicenseOnlys");
        }
    }
}
