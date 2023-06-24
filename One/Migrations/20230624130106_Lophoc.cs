using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace One.Migrations
{
    /// <inheritdoc />
    public partial class Lophoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lophoc",
                columns: table => new
                {
                    Malop = table.Column<string>(type: "TEXT", nullable: false),
                    Hoten = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lophoc", x => x.Malop);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lophoc");
        }
    }
}
