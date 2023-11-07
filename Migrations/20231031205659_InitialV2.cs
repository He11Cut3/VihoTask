using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VihoTask.Migrations
{
    public partial class InitialV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "VUserPhoto",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VUserPhoto",
                table: "AspNetUsers");
        }
    }
}
