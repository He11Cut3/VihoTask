using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VihoTask.Migrations
{
    public partial class InitialV12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VTaskFileExtension",
                table: "VTasks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VTaskFileExtension",
                table: "VTasks");
        }
    }
}
