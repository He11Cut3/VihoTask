using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VihoTask.Migrations
{
    public partial class InitialV13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VTaskContactUss",
                columns: table => new
                {
                    VTaskContactUsID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VTaskContactUsName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VTaskContactUsEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VTaskContactUsMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VTaskContactUss", x => x.VTaskContactUsID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VTaskContactUss");
        }
    }
}
