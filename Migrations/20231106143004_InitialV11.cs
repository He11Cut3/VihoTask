using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VihoTask.Migrations
{
    public partial class InitialV11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VTasks_AspNetUsers_VUserId",
                table: "VTasks");

            migrationBuilder.RenameColumn(
                name: "VUserId",
                table: "VTasks",
                newName: "VUserID");

            migrationBuilder.RenameIndex(
                name: "IX_VTasks_VUserId",
                table: "VTasks",
                newName: "IX_VTasks_VUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_VTasks_AspNetUsers_VUserID",
                table: "VTasks",
                column: "VUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VTasks_AspNetUsers_VUserID",
                table: "VTasks");

            migrationBuilder.RenameColumn(
                name: "VUserID",
                table: "VTasks",
                newName: "VUserId");

            migrationBuilder.RenameIndex(
                name: "IX_VTasks_VUserID",
                table: "VTasks",
                newName: "IX_VTasks_VUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_VTasks_AspNetUsers_VUserId",
                table: "VTasks",
                column: "VUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
