using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace File_Automation.Migrations
{
    public partial class new4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Uploads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName= table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Operation= table.Column<string>(type: "nvarchar(max)", nullable: true),
                    environment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    storage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestContainer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AzFolderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    currentDateTime=table.Column<DateTime>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uploads", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
