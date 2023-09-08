using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoStreaming.Migrations
{
    public partial class RemoveContentIdFromRooms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentId",
                table: "Rooms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentId",
                table: "Rooms",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
