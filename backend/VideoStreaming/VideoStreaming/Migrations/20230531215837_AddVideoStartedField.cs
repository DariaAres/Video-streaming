using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoStreaming.Migrations
{
    public partial class AddVideoStartedField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "VideoStarted",
                table: "Rooms",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoStarted",
                table: "Rooms");
        }
    }
}
