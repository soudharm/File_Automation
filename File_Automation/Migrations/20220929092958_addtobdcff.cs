using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace File_Automation.Migrations
{
    public partial class addtobdcff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Operation",
                table: "Uploads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Operation",
                table: "Uploads");
        }
    }
}
