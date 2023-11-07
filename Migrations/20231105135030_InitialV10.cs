using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VihoTask.Migrations
{
    public partial class InitialV10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMyMessage",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMyMessage",
                table: "Messages");
        }
    }
}
