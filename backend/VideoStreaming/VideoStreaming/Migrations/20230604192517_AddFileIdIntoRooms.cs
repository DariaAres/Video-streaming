using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoStreaming.Migrations
{
    public partial class AddFileIdIntoRooms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "Rooms",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_FileId",
                table: "Rooms",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_FilesData_FileId",
                table: "Rooms",
                column: "FileId",
                principalTable: "FilesData",
                principalColumn: "FileId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_FilesData_FileId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_FileId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Rooms");
        }
    }
}
