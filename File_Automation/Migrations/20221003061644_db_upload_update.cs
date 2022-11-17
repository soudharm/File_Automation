using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace File_Automation.Migrations
{
    public partial class db_upload_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Processed",
                table: "Uploads",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Processed",
                table: "Uploads");
        }
    }
}
