using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DOTP_BE.Migrations
{
    public partial class addedSomeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Summaries",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Summaries");
        }
    }
}
