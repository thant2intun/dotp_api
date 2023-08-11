using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DOTP_BE.Migrations
{
    public partial class addedColumnsIntoModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Temp_AttachedFile1",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Temp_AttachedFile2",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Temp_OwnerBook",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Temp_Township_Name",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Temp_Triangle",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Temp_VehicleBrand",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Temp_VehicleOwnerAddress",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Temp_VehicleOwnerNRC",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Temp_VehicleOwnerName",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Temp_VehicleType",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Temp_VehicleWeight",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Temp_Address",
                table: "LicenseOnlys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Temp_AttachFile_M10",
                table: "LicenseOnlys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Temp_AttachFile_NRC",
                table: "LicenseOnlys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Temp_AttachFile_OperatorLicense",
                table: "LicenseOnlys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Temp_AttachFile_Part1",
                table: "LicenseOnlys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Temp_Township_Name",
                table: "LicenseOnlys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "CreateCars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Temp_AttachedFile1",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Temp_AttachedFile2",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Temp_OwnerBook",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Temp_Township_Name",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Temp_Triangle",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Temp_VehicleBrand",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Temp_VehicleOwnerAddress",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Temp_VehicleOwnerNRC",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Temp_VehicleOwnerName",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Temp_VehicleType",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Temp_VehicleWeight",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Temp_Address",
                table: "LicenseOnlys");

            migrationBuilder.DropColumn(
                name: "Temp_AttachFile_M10",
                table: "LicenseOnlys");

            migrationBuilder.DropColumn(
                name: "Temp_AttachFile_NRC",
                table: "LicenseOnlys");

            migrationBuilder.DropColumn(
                name: "Temp_AttachFile_OperatorLicense",
                table: "LicenseOnlys");

            migrationBuilder.DropColumn(
                name: "Temp_AttachFile_Part1",
                table: "LicenseOnlys");

            migrationBuilder.DropColumn(
                name: "Temp_Township_Name",
                table: "LicenseOnlys");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CreateCars");
        }
    }
}
