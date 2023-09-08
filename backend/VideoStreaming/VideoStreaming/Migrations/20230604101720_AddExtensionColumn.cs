using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoStreaming.Migrations
{
    public partial class AddExtensionColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "FilesData",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extension",
                table: "FilesData");
        }
    }
}
