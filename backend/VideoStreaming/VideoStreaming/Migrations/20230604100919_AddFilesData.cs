using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoStreaming.Migrations
{
    public partial class AddFilesData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FilesData",
                columns: table => new
                {
                    FileId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilesData", x => x.FileId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilesData");
        }
    }
}
