using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DOTP_BE.Migrations
{
    public partial class AddSummaryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Summaries",
                columns: table => new
                {
                    SId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicenseNumberLong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalCar = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summaries", x => x.SId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Summaries");
        }
    }
}
