using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DOTP_BE.Migrations
{
    public partial class update_repre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RepresentativeId",
                table: "Vehicles",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ApplyLicenseType",
                table: "OperatorDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Representative",
                columns: table => new
                {
                    RepresentativeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NRC_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NRCId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Representative", x => x.RepresentativeId);
                    table.ForeignKey(
                        name: "FK_Representative_NRCs_NRCId",
                        column: x => x.NRCId,
                        principalTable: "NRCs",
                        principalColumn: "NRCId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_RepresentativeId",
                table: "Vehicles",
                column: "RepresentativeId");

            migrationBuilder.CreateIndex(
                name: "IX_Representative_NRCId",
                table: "Representative",
                column: "NRCId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Representative_RepresentativeId",
                table: "Vehicles",
                column: "RepresentativeId",
                principalTable: "Representative",
                principalColumn: "RepresentativeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Representative_RepresentativeId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "Representative");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_RepresentativeId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "RepresentativeId",
                table: "Vehicles");

            migrationBuilder.AlterColumn<string>(
                name: "ApplyLicenseType",
                table: "OperatorDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
