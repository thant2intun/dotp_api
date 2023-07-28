using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DOTP_BE.Migrations
{
    public partial class updateColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "L_Township_Name",
                table: "Temp_Tables",
                newName: "L_O_Township_Name");

            migrationBuilder.RenameColumn(
                name: "L_Address",
                table: "Temp_Tables",
                newName: "L_O_Address");

            migrationBuilder.AddColumn<string>(
                name: "L_N_Address",
                table: "Temp_Tables",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "L_N_Township_Name",
                table: "Temp_Tables",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "L_N_Address",
                table: "Temp_Tables");

            migrationBuilder.DropColumn(
                name: "L_N_Township_Name",
                table: "Temp_Tables");

            migrationBuilder.RenameColumn(
                name: "L_O_Township_Name",
                table: "Temp_Tables",
                newName: "L_Township_Name");

            migrationBuilder.RenameColumn(
                name: "L_O_Address",
                table: "Temp_Tables",
                newName: "L_Address");
        }
    }
}
